<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
	version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:umbraco.library="urn:umbraco.library"
	exclude-result-prefixes="msxml umbraco.library">


<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:param name="currentPage"/>

<xsl:variable name="numberOfItems">
	<xsl:choose>
		<xsl:when test="/macro/numberOfItems != ''">
			<xsl:value-of select="/macro/numberOfItems"/>
		</xsl:when>
		<xsl:otherwise>10</xsl:otherwise>
	</xsl:choose>
</xsl:variable>
<xsl:variable name="excerptLength">
	<xsl:choose>
		<xsl:when test="string(/macro/excerptLength) != ''">
			<xsl:value-of select="/macro/excerptLength"/>
		</xsl:when>
		<xsl:otherwise>0</xsl:otherwise>
	</xsl:choose>
</xsl:variable>

<xsl:variable name="feed" select="/macro/feedUrl"/>
<!-- cache for 30 minutes (1.800 seconds) -->
<xsl:variable name="cacheRate" select="number(1800)"/>

<xsl:template match="/">

<!-- start writing XSLT -->
<xsl:choose>
	<xsl:when test="$feed != ''">
		<xsl:variable name="feedContent" select="umbraco.library:GetXmlDocumentByUrl($feed, number($cacheRate))"/>
		<xsl:choose>
			<xsl:when test="$feedContent != 'error'">
				<xsl:call-template name="renderFeed">
					<xsl:with-param name="feedContent" select="$feedContent"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<p>
					<strong>Feed Viewer Macro Error: Error fetching feed</strong><br />
					The feed '<xsl:value-of select="$feed"/>' could not be loaded. Verify that the feed url exists and that you have an
					active internet connection
				</p>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:when>
	<xsl:otherwise>
		<p>
			<strong>Feed Viewer Macro Error: No feed chosen</strong><br />
			Please make sure to add a value in the "Feed Url" parameter
		</p>
	</xsl:otherwise>
</xsl:choose>
</xsl:template>

<xsl:template name="renderFeed">
<xsl:param name="feedContent"/>
<xsl:if test="count($feedContent//item) &gt; 0">
<ul class="feedList">
	<xsl:for-each select="$feedContent//item">
		<xsl:if test="position() &lt;= $numberOfItems">
			<li>
				<h4>
					<a href="{link}"><xsl:value-of select="title"/></a>
				</h4>
				<xsl:choose>
					<xsl:when test="string($excerptLength) != '0'">
						<p>
							<xsl:value-of select="umbraco.library:TruncateString(umbraco.library:StripHtml(description), number($excerptLength), '...')" disable-output-escaping="yes"/>
						</p>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="description" disable-output-escaping="yes"/>
					</xsl:otherwise>
				</xsl:choose>
				<small><xsl:value-of select="umbraco.library:LongDate(pubDate)"/></small>
			</li>
		</xsl:if>
	</xsl:for-each>
</ul>
</xsl:if>
</xsl:template>

</xsl:stylesheet>
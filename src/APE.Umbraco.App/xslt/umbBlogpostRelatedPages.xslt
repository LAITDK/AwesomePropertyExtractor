<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library"
  xmlns:BlogLibrary="urn:BlogLibrary"
  xmlns:tagsLib="urn:tagsLib"
  exclude-result-prefixes="msxml umbraco.library BlogLibrary">


  <xsl:output method="html" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>


    <xsl:template match="/">
        <xsl:if test="string-length($currentPage/tags) &gt; 0">

            <xsl:variable name="relatedposts" select="tagsLib:getContentsWithTags($currentPage/tags)/root/umbBlogPost [@id != $currentPage/@id]" />

            <xsl:if test="count($relatedposts) &gt; 0">

                <h3 class="commentsheader">Related posts</h3>

                <ul class="relatedPostsList">
                    <xsl:for-each select="$relatedposts">
                     <xsl:sort select="BlogLibrary:CountSeparatedStringMatches($currentPage/tags, ',', tags, ',')" order="descending" data-type="number"/>
                     <xsl:sort select="@createDate" order="descending" data-type="text"/>
                        <xsl:if test="position() &lt; 6">
                            <li>
                                <a href="{umbraco.library:NiceUrl(@id)}">
                                    <xsl:value-of select="@nodeName"/>
                                </a>
                            </li>
                        </xsl:if>
                    </xsl:for-each>
                </ul>
            </xsl:if>
        </xsl:if>
    </xsl:template>

</xsl:stylesheet>
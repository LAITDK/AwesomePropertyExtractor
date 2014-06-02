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
  exclude-result-prefixes="msxml umbraco.library BlogLibrary">


  <xsl:output method="html" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>
  <xsl:variable name="numberOfComments" select="10"/>
  <xsl:variable name="comments" select="BlogLibrary:GetCommentsForBlog($currentPage/ancestor-or-self::umbBlog/@id)//comment" />
 
  <xsl:template match="/">
  
<xsl:if test="count($comments) &gt; 0">
    <ul>
   <xsl:for-each select="$comments">
        <xsl:sort select="@created" order="descending" />
        <xsl:if test="position() &lt; $numberOfComments +1">

          <li class="comment-item">
            <a href="{umbraco.library:NiceUrl(@nodeid)}#comment-{@id}">
              <xsl:value-of select="./name"/> on <xsl:value-of select="umbraco.library:GetXmlNodeById(@nodeid)/@nodeName"/>
            </a>
          </li>
        </xsl:if>
      </xsl:for-each>
    </ul>
</xsl:if>

  </xsl:template>

</xsl:stylesheet>
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
  version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library"
  xmlns:tagsLib="urn:tagsLib"
  exclude-result-prefixes="msxml umbraco.library tagsLib">


<xsl:output method="html" omit-xml-declaration="yes"/>

<xsl:param name="currentPage"/>

<xsl:variable name="blogRoot" select="$currentPage/ancestor-or-self::umbBlog/@id"/>

<xsl:template match="/">
<ul>
  <li class="cat-item"><a href="{umbraco.library:NiceUrl($blogRoot)}">All</a> <span>&nbsp;(<xsl:value-of select="count($currentPage/ancestor-or-self::umbBlog//umbBlogPost)"/>)</span></li>
  <xsl:for-each select="tagsLib:getAllTagsInGroup('default')/tags/tag">
        <li class="cat-link">
            <a href="{umbraco.library:NiceUrl($blogRoot)}?filterby={current()}"><xsl:value-of select="current()"/></a> (<xsl:value-of select="@nodesTagged"/>)
        </li>
  </xsl:for-each>
</ul>

</xsl:template>

</xsl:stylesheet>
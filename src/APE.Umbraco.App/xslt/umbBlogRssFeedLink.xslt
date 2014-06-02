<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library"
  exclude-result-prefixes="msxml umbraco.library">


  <xsl:output method="xml" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>

  <xsl:template match="/">

    <!-- xslt file that will output all rss feed links 
          - Latest posts rss feed
          - Latest posts rss feed for each tag
          - Comments rss feed      
          - Comments rss feed for each post
    
        FE:
        <link href="/blog/rss.aspx" rel="alternate" type="application/rss+xml" title="RSS Feed" />
        <link href="/blog/commentrss.aspx" rel="alternate" type="application/rss+xml" title="Comments RSS Feed" />
        
    -->
  
    <xsl:variable name="fixedHref" >
        <xsl:call-template name="fixRssHref">
          <xsl:with-param name="niceUrl">
            <xsl:value-of select="umbraco.library:NiceUrl($currentPage/ancestor-or-self::root//umbBlog/@id)" />
          </xsl:with-param>
      </xsl:call-template>
    </xsl:variable>
    
    <!-- This outputs the latest posts rss feed link-->
    <link href="{concat($fixedHref, '/rss.aspx')}" rel="alternate" type="application/rss+xml" title="RSS Feed" />
    
    <!-- This outputs the filtered blogpost (for a certain tag) rss feed link (will be active when filtering by tag)-->
    <xsl:if test="string-length(umbraco.library:Request('filterby')) &gt; 0">
      <link href="{concat($fixedHref, concat('/rss/tags/',umbraco.library:Request('filterby'),'.aspx'))}" rel="alternate" type="application/rss+xml" title="RSS Feed for tag {umbraco.library:Request('filterby')}" />
    </xsl:if>

    <!-- This outputs the main comments rss feed link -->
    <link href="{concat($fixedHref, '/commentrss.aspx')}" rel="alternate" type="application/rss+xml" title="Comments RSS Feed" />

    <!-- This outputs the comments rss feed link for a certain post (will only be active when on a post page)-->
    <xsl:if test="string($currentPage/umbBlogPost/closeComments) != '1'">
      <xsl:variable name="fixedPostHref" >
        <xsl:call-template name="fixRssHref">
          <xsl:with-param name="niceUrl">
            <xsl:value-of select="umbraco.library:NiceUrl($currentPage/@id)" />
          </xsl:with-param>
        </xsl:call-template>
      </xsl:variable>
      <link href="{concat($fixedPostHref, '/commentrss.aspx')}" rel="alternate" type="application/rss+xml" title="Comments RSS Feed for {$currentPage/@nodeName}" />
    </xsl:if>
  </xsl:template>

  <!-- Get correct URL root regardless of whether umbracoUseDirectoryUrls is true or false -->
  <xsl:template name="fixRssHref">
    <xsl:param name="niceUrl" />
    <xsl:variable name="indexOfAspx" select="umbraco.library:LastIndexOf($niceUrl, '.aspx')" />
    
    <xsl:variable name="fixedHref" >
      <xsl:choose>
        <xsl:when test="$indexOfAspx != -1">
          <xsl:value-of select="umbraco.library:Replace($niceUrl, '.aspx', '')" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$niceUrl" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:if test="$fixedHref != '/'">
    <xsl:value-of select="$fixedHref" />
    </xsl:if>
    
  </xsl:template>
</xsl:stylesheet>
<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxml="urn:schemas-microsoft-com:xslt"
xmlns:umbraco.library="urn:umbraco.library"
xmlns:rssdatehelper="urn:rssdatehelper"
xmlns:dc="http://purl.org/dc/elements/1.1/"
xmlns:content="http://purl.org/rss/1.0/modules/content/"
xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings"
xmlns:BlogLibrary="urn:BlogLibrary"
exclude-result-prefixes="msxml umbraco.library Exslt.ExsltStrings BlogLibrary">


  <xsl:output method="xml" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>

  <xsl:variable name="iscommentfeed" select="//macro/iscommentfeed" />

  <!-- Update these variables to modify the feed -->
  <xsl:variable name="RSSNoItems" select="string('10')"/>
  <xsl:variable name="RSSTitle">
    <xsl:value-of select="$currentPage/ancestor-or-self::umbBlog/siteName"/>
    <xsl:if test="$iscommentfeed = '1'">
      <xsl:text> comment feed</xsl:text>
      <xsl:if test="$currentPage [name() = 'umbBlogPost']">
        <xsl:value-of select="concat(' for ', $currentPage/@nodeName)"/>
      </xsl:if>
    </xsl:if>
    <xsl:if test="string-length(umbraco.library:Request('tag')) &gt; 0">
      <xsl:value-of select="concat(' for tag ', umbraco.library:Request('tag'))"/>
    </xsl:if>
  </xsl:variable>
  <xsl:variable name="SiteURL" select="concat('http://',string(umbraco.library:RequestServerVariables('HTTP_HOST')))"/>
  <xsl:variable name="RSSDescription" select="$currentPage/ancestor-or-self::umbBlog/siteDescription"/>

  <!-- This gets all news and events and orders by updateDate to use for the pubDate in RSS feed -->
  <xsl:variable name="pubDate">
    <xsl:choose>
      <xsl:when test="$iscommentfeed = '1'">
        
        <xsl:for-each select="$currentPage//umbBlogPostComment">
          <xsl:sort select="@createDate" data-type="text" order="descending" />
          <xsl:if test="position() = 1">
            <xsl:value-of select="@updateDate" />
          </xsl:if>
        </xsl:for-each>
        
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="string-length(umbraco.library:Request('tag')) &gt; 0">
            <xsl:for-each select="$currentPage//umbBlogPost [contains(Exslt.ExsltStrings:lowercase(./tags), Exslt.ExsltStrings:lowercase(umbraco.library:Request('tag')))]">
              <xsl:sort select="PostDate" data-type="text" order="descending" />
              <xsl:if test="position() = 1">
                <xsl:value-of select="@updateDate" />
              </xsl:if>
            </xsl:for-each>
          </xsl:when>
          <xsl:otherwise>
            <xsl:for-each select="$currentPage//umbBlogPost">
              <xsl:sort select="PostDate" data-type="text" order="descending" />
              <xsl:if test="position() = 1">
                <xsl:value-of select="@updateDate" />
              </xsl:if>
            </xsl:for-each>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:template match="/">


    <!-- change the mimetype for the current page to xml -->
    <xsl:value-of select="umbraco.library:ChangeContentType('text/xml')"/>

    <xsl:text disable-output-escaping="yes">&lt;?xml version="1.0" encoding="UTF-8"?&gt;</xsl:text>
    <rss version="2.0"
    xmlns:content="http://purl.org/rss/1.0/modules/content/"
    xmlns:wfw="http://wellformedweb.org/CommentAPI/"
    xmlns:dc="http://purl.org/dc/elements/1.1/"
>

      <channel>
        <title>
          <xsl:value-of select="$RSSTitle"/>
        </title>
        <link>
          <xsl:value-of select="$SiteURL"/>
        </link>
        <pubDate>
          <xsl:value-of select="$pubDate"/>
        </pubDate>
        <generator>umbraco</generator>
        <description>
          <xsl:value-of select="$RSSDescription"/>
        </description>
        <language>en</language>

        <xsl:choose>
          <xsl:when test="$iscommentfeed = '1'">

            <xsl:choose>
              <xsl:when test="$currentPage [name() = 'umbBlogPost']">
                <xsl:apply-templates select="BlogLibrary:GetCommentsForPost($currentPage/@id)//comment">
                  <xsl:sort select="@created" order="descending" />
                </xsl:apply-templates>
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates select="BlogLibrary:GetCommentsForBlog($currentPage/ancestor-or-self::umbBlog/@id)//comment">
                  <xsl:sort select="@created" order="descending" />
                </xsl:apply-templates>
              </xsl:otherwise>
            </xsl:choose>


          </xsl:when>
          <xsl:otherwise>

            <xsl:choose>
              <xsl:when test="string-length(umbraco.library:Request('tag')) &gt; 0">
                <xsl:apply-templates select="$currentPage//umbBlogPost [contains(Exslt.ExsltStrings:lowercase(./tags), Exslt.ExsltStrings:lowercase(umbraco.library:Request('tag')))]">
                  <xsl:sort select="PostDate" order="descending" />
                </xsl:apply-templates>
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates select="$currentPage//umbBlogPost">
                  <xsl:sort select="PostDate" order="descending" />
                </xsl:apply-templates>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:otherwise>
        </xsl:choose>

      </channel>
    </rss>

  </xsl:template>

  <xsl:template match="comment">
    <xsl:if test="position() &lt;= $RSSNoItems">
      <xsl:variable name="link">
        <xsl:value-of select="concat(umbraco.library:NiceUrl(@nodeid),'#comment-',@id)"/>
      </xsl:variable>
      <xsl:variable name="content">
        <xsl:value-of select="umbraco.library:ReplaceLineBreaks(./message)"/>
      </xsl:variable>
      <item>
        <title>
          Re <xsl:value-of select="umbraco.library:GetXmlNodeById(@nodeid)/@nodeName"/> by <xsl:value-of select="./name"/>
        </title>
        <link>
          <xsl:value-of select="$SiteURL"/>
          <xsl:value-of select="$link"/>
        </link>
        <pubDate>
          <xsl:value-of select="umbraco.library:FormatDateTime(PostDate,'r')" />
        </pubDate>
        <guid>
          <xsl:value-of select="$SiteURL"/>
          <xsl:value-of select="$link"/>
        </guid>
        <content:encoded>
          <xsl:value-of select="concat('&lt;![CDATA[ ', $content,']]&gt;')" disable-output-escaping="yes"/>
        </content:encoded>
      </item>
    </xsl:if>
  </xsl:template>

  <xsl:template match="umbBlogPost">
    <xsl:if test="position() &lt;= $RSSNoItems">
      <xsl:variable name="link">
        <xsl:value-of select="umbraco.library:NiceUrl(@id)"/>
      </xsl:variable>
      <xsl:variable name="content">
        <xsl:value-of select="./bodyText"/>
      </xsl:variable>
      <item>
        <title>
          <xsl:value-of select="@nodeName"/>
        </title>
        <link>
          <xsl:value-of select="$SiteURL"/>
          <xsl:value-of select="$link"/>
        </link>
        <pubDate>
          <xsl:value-of select="umbraco.library:FormatDateTime(PostDate,'r')" />
        </pubDate>
        <guid>
          <xsl:value-of select="$SiteURL"/>
          <xsl:value-of select="$link"/>
        </guid>
        <description>
          <xsl:value-of select="concat('&lt;![CDATA[ ', $content,']]&gt;')" disable-output-escaping="yes"/>
        </description>
      </item>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
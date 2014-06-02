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

    <xsl:template match="/">

        <xsl:variable name="comments" select="BlogLibrary:GetCommentsForPost($currentPage/@id)//comment"/>
        <xsl:if test="count($comments) &gt; 0">
            <h3>
              <xsl:value-of select="count($comments)"/> comment<xsl:if test="count($comments) &gt; 1">s</xsl:if>  <span><em> on </em>  &#8220;<xsl:value-of select="$currentPage/@nodeName"/>&#8221;</span>
            </h3>

            <ol class="commentlist">
                <xsl:for-each select="$comments">

                    <li class="comment alt" id="comment-{@id}">
                        <div class="comment-author vcard">
                            <img class="photo avatar avatar-32 photo" width="32" height="32" src="{BlogLibrary:getGravatar(./email, 40, '')}" alt="Gravatar of {./name}"/>
                            <span class="fn n">
                                <xsl:choose>
                                    <xsl:when test="string-length(./website) = 0">
                                        <xsl:value-of select="./name"/>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <a class="url url" rel="external nofollow" href="{./website}">
                                            <xsl:value-of select="./name"/>
                                        </a>
                                    </xsl:otherwise>
                                </xsl:choose>

                            </span>
                        </div>
                        <div class="comment-meta">
                            Posted <xsl:value-of select="umbraco.library:LongDate(@created,'true',' at ')"/>
                        </div>
                        <p>
                            <xsl:value-of select="umbraco.library:ReplaceLineBreaks( umbraco.library:HtmlEncode( ./message ))" disable-output-escaping="yes"/>
                        </p>

                    </li>
                </xsl:for-each>
            </ol>
        </xsl:if>
    </xsl:template>

</xsl:stylesheet>
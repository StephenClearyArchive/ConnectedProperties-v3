<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <!-- Copy all documentation as-is except for what matches other rules -->
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <!-- Convert "cref" references that start with "O:" to starting with "Overload:". -->
  <xsl:template match="@cref[starts-with(., 'O:')]">
    <xsl:attribute name="cref">
      <xsl:value-of select="concat('Overload:', substring-after(., 'O:'))"/>
    </xsl:attribute>
  </xsl:template>
</xsl:stylesheet>

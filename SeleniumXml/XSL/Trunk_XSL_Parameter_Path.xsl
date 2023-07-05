<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output encoding="UTF-8" indent="yes" method="html" standalone="no" omit-xml-declaration="no"/>
	<!-- Copy of all attributes -->
	<xsl:template match="node()|@*">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
	</xsl:template>
	<!-- 1 Adding parent node  -->
	<xsl:template match="SelectEntity/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="SelectEntityBox/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
		<xsl:copy-of select="./Actions"/>
	</xsl:template>
	
	<xsl:template match="Edit/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="Rename/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			<xsl:copy-of select="./newName"/>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="Renaming/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="RenameF2/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			<xsl:copy-of select="./newName"/>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="AddFolder/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	
	<xsl:template match="DragAndDrop/Parameters">
         <Parameters>
			<dragNodes><path><xsl:value-of select="./dragPath"/></path></dragNodes>
			<dropNodes><path><xsl:value-of select="./dropPath"/></path></dropNodes>
			<xsl:copy-of select="./asChild"/>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="CompareTheObject/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="EntitytPresence/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			<xsl:copy-of select="./presence"/>
		</Parameters>
		
	</xsl:template>
	
	<xsl:template match="AdvancedDuplicate/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			<xsl:copy-of select="./advDuplicateName"/>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="ObjectPopUpMenu/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			<xsl:copy-of select="./modelName"/>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="Properties/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="SelectEntity/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="DoubleClickEntity/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="ReadEntityRow/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			<xsl:copy-of select="./ExpectedValue"/>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="AddObject/Parameters">
	
	<xsl:variable name="path">
	<xsl:value-of select="./path"/>
	</xsl:variable>
	<xsl:choose>
	<xsl:when test="path!=''">
	<Parameters>
	<nodes>
    <xsl:copy-of select="./path"/>		
    </nodes>
	</Parameters>
	</xsl:when>
	<xsl:otherwise>
	<Parameters>
	</Parameters>
	</xsl:otherwise>
	</xsl:choose>
	</xsl:template>
	
	<xsl:template match="SortEntireTree/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			<xsl:copy-of select="./sortType"/>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="AddLinkedObject/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			</Parameters>
			<xsl:copy-of select="./Actions"/>        
	</xsl:template>
	
	<xsl:template match="Duplicate/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			</Parameters>
			<xsl:copy-of select="./Actions"/>        
	</xsl:template>
	
	<xsl:template match="DeleteLinkedObject/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			</Parameters>
			<xsl:copy-of select="./Actions"/>        
	</xsl:template>
	
	<xsl:template match="ExpandCollapseEntity/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
        </Parameters>
	</xsl:template>
	
	<xsl:template match="ModificationsHistory/Parameters">
         <Parameters>
			<nodes>
			<xsl:copy-of select="./path"/>
			</nodes>
			</Parameters>
			<xsl:copy-of select="./Actions"/>        
	</xsl:template>
	
	<!-- 2 Rewrite expected results for wegcheckbox -->
	<!--<xsl:template match="Read/Parameters">
		<ExpectedResult>
			<xsl:value-of select="//expectedResult"/>
		</ExpectedResult>
	</xsl:template> -->
	
	<!-- 3 Rename Tag-->
	<!--<xsl:template match="CompressFile">
		<CompressFileBis><xsl:apply-templates select="@*|node()" /></CompressFileBis>
	</xsl:template> -->
	
	<!-- Rename Tag Second version-->
	<!-- <xsl:template match="CompressFile">
        <CompressFileBis><xsl:copy-of select="./Actions"/></CompressFileBis>
    </xsl:template> -->
</xsl:stylesheet>

using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Mapping.Labeling;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ArcGISTestRenderer
{
    public static class ContactLabelsDefinition
    {
        private const int TRANSPARENCY_FACTOR = 85;

        private static Color Unknown = Color.LightGoldenrodYellow;
        private static Color Neutral = Color.LightGreen;
        private static Color Friend  = Color.LightCyan;
        private static Color Hostile = Color.LightSalmon;

        #region TextSymbols

        #region Neutral

        public static TextSymbol NeutrallabelSymbol1 = new TextSymbol
        {
            Color = Neutral,
            BackgroundColor = Color.Transparent,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        public static TextSymbol NeutrallabelSymbol2 = new TextSymbol
        {
            Color = Neutral,
            BackgroundColor = Color.FromArgb(TRANSPARENCY_FACTOR, Neutral),
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };
        
        public static TextSymbol NeutrallabelSymbol3 = new TextSymbol
        {
            Color = Color.Black,
            BackgroundColor = Color.FromArgb(TRANSPARENCY_FACTOR*2, Neutral),
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };
        
        public static TextSymbol NeutrallabelSymbol4 = new TextSymbol
        {
            Color = Color.Black,
            BackgroundColor = Neutral,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        #endregion Neutral

        #region Unknown

        public static TextSymbol UnknownlabelSymbol1 = new TextSymbol
        {
            Color = Unknown,
            BackgroundColor = Color.Transparent,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        public static TextSymbol UnknownlabelSymbol2 = new TextSymbol
        {
            Color = Unknown,
            BackgroundColor = Color.FromArgb(TRANSPARENCY_FACTOR, Unknown),
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };
        
        public static TextSymbol UnknownlabelSymbol3 = new TextSymbol
        {
            Color = Color.Black,
            BackgroundColor = Color.FromArgb(TRANSPARENCY_FACTOR*2, Unknown),
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };
        
        public static TextSymbol UnknownlabelSymbol4 = new TextSymbol
        {
            Color = Color.Black,
            BackgroundColor = Unknown,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        #endregion Unknown
        
        #region Friend

        public static TextSymbol FriendlabelSymbol1 = new TextSymbol
        {
            Color = Friend,
            BackgroundColor = Color.Transparent,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        public static TextSymbol FriendlabelSymbol2 = new TextSymbol
        {
            Color = Friend,
            BackgroundColor = Color.FromArgb(TRANSPARENCY_FACTOR, Friend),
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };
        
        public static TextSymbol FriendlabelSymbol3 = new TextSymbol
        {
            Color = Color.Black,
            BackgroundColor = Color.FromArgb(TRANSPARENCY_FACTOR*2, Friend),
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };
        
        public static TextSymbol FriendlabelSymbol4 = new TextSymbol
        {
            Color = Color.Black,
            BackgroundColor = Friend,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        #endregion Friend
        
        #region Hostile

        public static TextSymbol HostilelabelSymbol1 = new TextSymbol
        {
            Color = Hostile,
            BackgroundColor = Color.Transparent,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        public static TextSymbol HostilelabelSymbol2 = new TextSymbol
        {
            Color = Hostile,
            BackgroundColor = Color.FromArgb(TRANSPARENCY_FACTOR, Hostile),
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };
        
        public static TextSymbol HostilelabelSymbol3 = new TextSymbol
        {
            Color = Color.Black,
            BackgroundColor = Color.FromArgb(TRANSPARENCY_FACTOR*2, Hostile),
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };
        
        public static TextSymbol HostilelabelSymbol4 = new TextSymbol
        {
            Color = Color.Black,
            BackgroundColor = Hostile,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        #endregion Hostile

        #endregion Textsymbols

        #region LabelDefinitions

        #region Neutral

        public static LabelDefinition NeutralLabelDef1 = new LabelDefinition(new SimpleLabelExpression("[label]"), NeutrallabelSymbol1)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'neutral1'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition NeutralLabelDef2 = new LabelDefinition(new SimpleLabelExpression("[label]"), NeutrallabelSymbol2)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'neutral2'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };
        
        public static LabelDefinition NeutralLabelDef3 = new LabelDefinition(new SimpleLabelExpression("[label]"), NeutrallabelSymbol3)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'neutral3'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };
        
        public static LabelDefinition NeutralLabelDef4 = new LabelDefinition(new SimpleLabelExpression("[label]"), NeutrallabelSymbol4)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'neutral4'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        #endregion Neutral

        #region Unknown

        public static LabelDefinition UnknownLabelDef1 = new LabelDefinition(new SimpleLabelExpression("[label]"), UnknownlabelSymbol1)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'unknown1'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition UnknownLabelDef2 = new LabelDefinition(new SimpleLabelExpression("[label]"), UnknownlabelSymbol2)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'unknown2'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition UnknownLabelDef3 = new LabelDefinition(new SimpleLabelExpression("[label]"), UnknownlabelSymbol3)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'unknown3'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition UnknownLabelDef4 = new LabelDefinition(new SimpleLabelExpression("[label]"), UnknownlabelSymbol4)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'unknown4'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        #endregion Unknown

        #region Friend

        public static LabelDefinition FriendLabelDef1 = new LabelDefinition(new SimpleLabelExpression("[label]"), FriendlabelSymbol1)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'friend1'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition FriendLabelDef2 = new LabelDefinition(new SimpleLabelExpression("[label]"), FriendlabelSymbol2)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'friend2'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition FriendLabelDef3 = new LabelDefinition(new SimpleLabelExpression("[label]"), FriendlabelSymbol3)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'friend3'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition FriendLabelDef4 = new LabelDefinition(new SimpleLabelExpression("[label]"), FriendlabelSymbol4)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'friend4'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        #endregion Friend

        #region Hostile

        public static LabelDefinition HostileLabelDef1 = new LabelDefinition(new SimpleLabelExpression("[label]"), HostilelabelSymbol1)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'hostile1'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition HostileLabelDef2 = new LabelDefinition(new SimpleLabelExpression("[label]"), HostilelabelSymbol2)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'hostile2'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition HostileLabelDef3 = new LabelDefinition(new SimpleLabelExpression("[label]"), HostilelabelSymbol3)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'hostile3'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static LabelDefinition HostileLabelDef4 = new LabelDefinition(new SimpleLabelExpression("[label]"), HostilelabelSymbol4)
        {
            WhereClause = "[graphicType] = 'labelLine' AND [labelColor] = 'hostile4'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        #endregion Hostile

        #endregion LabelDefinitions
    }
}

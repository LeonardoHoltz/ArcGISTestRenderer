using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Mapping.Labeling;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISTestRenderer
{
    public static class ContactLabelsDefinition
    {

        public static TextSymbol labelSymbol = new TextSymbol
        {
            Color = System.Drawing.Color.Black,
            BackgroundColor = System.Drawing.Color.Transparent,
            FontFamily = "Arial",
            Size = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            OffsetY = -30,
        };

        public static LabelDefinition labelDef = new LabelDefinition(new SimpleLabelExpression("[label]"), labelSymbol)
        {
            WhereClause = "[graphicType] = 'labelLine'",
            Placement = Esri.ArcGISRuntime.ArcGISServices.LabelingPlacement.PointCenterCenter,
            TextLayout = LabelTextLayout.Horizontal,
            OverrunStrategy = LabelOverrunStrategy.Allow,
            RemoveDuplicatesStrategy = LabelRemoveDuplicatesStrategy.None,
            RepeatStrategy = LabelRepeatStrategy.Repeat,
            LineConnection = LabelLineConnection.None,
            LabelOverlapStrategy = LabelOverlapStrategy.Allow,
        };

        public static void ChangeLabelColors(System.Drawing.Color textColor, System.Drawing.Color backgroundColor)
        {
            labelSymbol.Color = textColor;
            labelSymbol.BackgroundColor = backgroundColor;
        }
    }
}

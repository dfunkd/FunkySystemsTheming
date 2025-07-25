﻿using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace RecipesWithFunk.CustomControls;

[ContentProperty("Text")]
public class OutlinedTextBlock : FrameworkElement
{
    private FormattedText formattedText;
    private Geometry textGeometry;

    #region Dependency Proerties
    #region Fill Depenedency Property
    public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
    public Brush Fill
    {
        get => (Brush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }
    #endregion

    #region FontFamily Depenedency Property
    public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextUpdated));
    public FontFamily FontFamily
    {
        get => (FontFamily)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }
    #endregion

    #region FontSize Depenedency Property
    public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextUpdated));
    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }
    #endregion

    #region FontStretch Depenedency Property
    public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextUpdated));
    public FontStretch FontStretch
    {
        get => (FontStretch)GetValue(FontStretchProperty);
        set => SetValue(FontStretchProperty, value);
    }
    #endregion

    #region fontStyle Depenedency Property
    public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextUpdated));
    public FontStyle FontStyle
    {
        get => (FontStyle)GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }
    #endregion

    #region FontWeight Depenedency Property
    public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextUpdated));
    public FontWeight FontWeight
    {
        get => (FontWeight)GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }
    #endregion

    #region Stroke Depenedency Property
    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
    public Brush Stroke
    {
        get => (Brush)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }
    #endregion

    #region StrokeThickness Depenedency Property
    public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));
    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }
    #endregion

    #region Text Depenedency Property
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextInvalidated));
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    #endregion

    #region TextAlignment Depenedency Property
    public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextUpdated));
    public TextAlignment TextAlignment
    {
        get => (TextAlignment)GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }
    #endregion

    #region TextDecorations Depenedency Property
    public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextUpdated));
    public TextDecorationCollection TextDecorations
    {
        get => (TextDecorationCollection)GetValue(TextDecorationsProperty);
        set => SetValue(TextDecorationsProperty, value);
    }
    #endregion

    #region TextTrimming Depenedency Property
    public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(OnFormattedTextUpdated));
    public TextTrimming TextTrimming
    {
        get => (TextTrimming)GetValue(TextTrimmingProperty);
        set => SetValue(TextTrimmingProperty, value);
    }
    #endregion

    #region TextWrapping Depenedency Property
    public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(OutlinedTextBlock),
        new FrameworkPropertyMetadata(TextWrapping.NoWrap, OnFormattedTextUpdated));
    public TextWrapping TextWrapping
    {
        get => (TextWrapping)GetValue(TextWrappingProperty);
        set => SetValue(TextWrappingProperty, value);
    }
    #endregion
    #endregion

    public OutlinedTextBlock()
    {
        TextDecorations = new TextDecorationCollection();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        EnsureGeometry();

        drawingContext.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness), textGeometry);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        EnsureFormattedText();

        // constrain the formatted text according to the available size
        // the Math.Min call is important - without this constraint (which seems arbitrary, but is the maximum allowable text width),
        //      things blow up when availableSize is infinite in both directions
        // the Math.Max call is to ensure we don't hit zero, which will cause MaxTextHeight to throw
        formattedText.MaxTextWidth = Math.Min(3579139, availableSize.Width);
        formattedText.MaxTextHeight = Math.Max(0.0001d, availableSize.Height);

        // return the desired size
        return new Size(formattedText.Width, formattedText.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        EnsureFormattedText();

        // update the formatted text with the final size
        formattedText.MaxTextWidth = finalSize.Width;
        formattedText.MaxTextHeight = finalSize.Height;

        // need to re-generate the geometry now that the dimensions have changed
        textGeometry = null;

        return finalSize;
    }

    private static void OnFormattedTextInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        var outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
        outlinedTextBlock.formattedText = null;
        outlinedTextBlock.textGeometry = null;

        outlinedTextBlock.InvalidateMeasure();
        outlinedTextBlock.InvalidateVisual();
    }

    private static void OnFormattedTextUpdated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        var outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
        outlinedTextBlock.UpdateFormattedText();
        outlinedTextBlock.textGeometry = null;

        outlinedTextBlock.InvalidateMeasure();
        outlinedTextBlock.InvalidateVisual();
    }

    private void EnsureFormattedText()
    {
        if (formattedText is not null || Text is null)
            return;

        formattedText = new FormattedText(Text, CultureInfo.CurrentUICulture, FlowDirection,
            new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal), FontSize, Brushes.Black);

        UpdateFormattedText();
    }

    private void UpdateFormattedText()
    {
        if (formattedText is null)
            return;

        formattedText.MaxLineCount = TextWrapping == TextWrapping.NoWrap ? 1 : int.MaxValue;
        formattedText.TextAlignment = TextAlignment;
        formattedText.Trimming = TextTrimming;

        formattedText.SetFontSize(FontSize);
        formattedText.SetFontStyle(FontStyle);
        formattedText.SetFontWeight(FontWeight);
        formattedText.SetFontFamily(FontFamily);
        formattedText.SetFontStretch(FontStretch);
        formattedText.SetTextDecorations(TextDecorations);
    }

    private void EnsureGeometry()
    {
        if (textGeometry is not null)
            return;

        EnsureFormattedText();
        textGeometry = formattedText.BuildGeometry(new Point(0, 0));
    }
}

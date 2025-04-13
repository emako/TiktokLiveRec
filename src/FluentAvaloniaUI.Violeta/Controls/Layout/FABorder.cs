using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using Avalonia.Layout;
using Avalonia.Utilities;
using Avalonia.Media.Immutable;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace FluentAvalonia.UI.Controls;

// https://github.com/amwx/FluentAvalonia/blob/v2.0.4/src/FluentAvalonia/UI/Controls/FABorder/FABorder.cs
// See /Internal/BorderRenderHelper.cs for a note on some changes made compared to upstream
// Border and BorderRenderHelper

/// <summary>
/// Border control that allows specifying how the border and background align when rendered
/// </summary>
public class FABorder : Decorator
{
    /// <summary>
    /// Defines the <see cref="Background"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush> BackgroundProperty =
        AvaloniaProperty.Register<Border, IBrush>(nameof(Background));

    /// <summary>
    /// Defines the <see cref="BorderBrush"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush> BorderBrushProperty =
        AvaloniaProperty.Register<Border, IBrush>(nameof(BorderBrush));

    /// <summary>
    /// Defines the <see cref="BorderThickness"/> property.
    /// </summary>
    public static readonly StyledProperty<Thickness> BorderThicknessProperty =
        AvaloniaProperty.Register<Border, Thickness>(nameof(BorderThickness));

    /// <summary>
    /// Defines the <see cref="CornerRadius"/> property.
    /// </summary>
    public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
        AvaloniaProperty.Register<Border, CornerRadius>(nameof(CornerRadius));

    /// <summary>
    /// Defines the <see cref="BoxShadow"/> property.
    /// </summary>
    public static readonly StyledProperty<BoxShadows> BoxShadowProperty =
        AvaloniaProperty.Register<Border, BoxShadows>(nameof(BoxShadow));

    /// <summary>
    /// Defines the <see cref="BackgroundSizing"/> property
    /// </summary>
    public static readonly StyledProperty<BackgroundSizing> BackgroundSizingProperty =
        AvaloniaProperty.Register<FABorder, BackgroundSizing>(nameof(BackgroundSizing));

    /// <summary>
    /// Gets or sets a brush with which to paint the background.
    /// </summary>
    public IBrush Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    /// <summary>
    /// Gets or sets a brush with which to paint the border.
    /// </summary>
    public IBrush BorderBrush
    {
        get => GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness of the border.
    /// </summary>
    public Thickness BorderThickness
    {
        get => GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    /// <summary>
    /// Gets or sets the radius of the border rounded corners.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the box shadow effect parameters
    /// </summary>
    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that indicates how far the background extends in relation to this element's border.
    /// </summary>
    public BackgroundSizing BackgroundSizing
    {
        get => GetValue(BackgroundSizingProperty);
        set => SetValue(BackgroundSizingProperty, value);
    }

    private Thickness LayoutThickness
    {
        get
        {
            VerifyScale();

            if (_layoutThickness == null)
            {
                var borderThickness = BorderThickness;

                if (UseLayoutRounding)
                    borderThickness = LayoutHelper.RoundLayoutThickness(borderThickness, _scale, _scale);

                _layoutThickness = borderThickness;
            }

            return _layoutThickness.Value;
        }
    }

    private void VerifyScale()
    {
        var currentScale = LayoutHelper.GetLayoutScale(this);
        if (MathUtilities.AreClose(currentScale, _scale))
            return;

        _scale = currentScale;
        _layoutThickness = null;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (_helper != null)
        {
            if (change.Property == BackgroundProperty)
            {
                _helper.Background = change.GetNewValue<IBrush>();
                InvalidateVisual();
            }
            else if (change.Property == BorderBrushProperty)
            {
                _helper.BorderBrush = change.GetNewValue<IBrush>();
                InvalidateVisual();
            }
            else if (change.Property == BorderThicknessProperty)
            {
                _helper.BorderThickness = change.GetNewValue<Thickness>();
                InvalidateMeasure();
            }
            else if (change.Property == CornerRadiusProperty)
            {
                _helper.CornerRadius = change.GetNewValue<CornerRadius>();
                InvalidateVisual();
            }
            else if (change.Property == BoxShadowProperty)
            {
                _helper.BoxShadow = change.GetNewValue<BoxShadows>();
                InvalidateVisual();
            }
            else if (change.Property == BackgroundSizingProperty)
            {
                _helper.BackgroundSizing = change.GetNewValue<BackgroundSizing>();
                InvalidateVisual();
            }
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return LayoutHelper.MeasureChild(Child, availableSize, Padding, BorderThickness);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        return LayoutHelper.ArrangeChild(Child, finalSize, Padding, BorderThickness);
    }

    public override void Render(DrawingContext context)
    {
        _helper ??= new BorderRenderHelper(Background, BorderBrush, BorderThickness,
            CornerRadius, BoxShadow, BackgroundSizing);

        _helper.Render(context, Bounds.Size);
    }

    private BorderRenderHelper _helper;
    private Thickness? _layoutThickness;
    private double _scale;
}

/// <summary>
/// Defines constants that specify how far an element's background extends in relation to the element's border.
/// </summary>
public enum BackgroundSizing
{
    /// <summary>
    /// The element's background extends to the inner edge of the border, but does not extend under the border.
    /// </summary>
    InnerBorderEdge,

    /// <summary>
    /// The element's background extends under the border to its outer edge, and is visible if the border is transparent.
    /// </summary>
    OuterBorderEdge
}

// This is a modified version of the BorderRenderHelper taken from the Avalonia source
// In addition to supporting BackgroundSizing for FABorder, I've also make some changes:
// 1- Cached the properties from the border here instead of passing them in on render
//    this save several StyledProperty reads ever OnRender call to populate the BRH,
//    even if the properties don't change. This is of course at the trade off of
//    the extra memory to store the local values to the FABorder
// 2- BorderGeometryKeypoints was changed from a class to a readonly struct
// 3- Added ComplexBorderRender class to store the complex render path geometries rather than
//    always have 2 StreamGeometry instances on the BRH, we only have 1 pointer if we need it
// 4- Removed support for the 'Pen' properties (e.g., DashStyle). IMO those properties don't
//    belong on Border in the first place, and they don't work with the complex render path
//    anyway.

internal class BorderRenderHelper
{
    public BorderRenderHelper(IBrush bg, IBrush bb, Thickness thickness, CornerRadius cr,
        BoxShadows bs, BackgroundSizing sizing)
    {
        Background = bg;
        BorderBrush = bb;
        BorderThickness = thickness;
        CornerRadius = cr;
        BoxShadow = bs;
        BackgroundSizing = sizing;
    }

    public IBrush Background { get; set; }

    public IBrush BorderBrush { get; set; }

    public Thickness BorderThickness
    {
        get => _borderThickness;
        set
        {
            _borderThickness = value;
            _initialized = false;
        }
    }

    public CornerRadius CornerRadius
    {
        get => _cornerRadius;
        set
        {
            _cornerRadius = value;
            _initialized = false;
        }
    }

    public BoxShadows BoxShadow { get; set; }

    public BackgroundSizing BackgroundSizing
    {
        get => _backgroundSizing;
        set
        {
            _backgroundSizing = value;
            _initialized = false;
        }
    }

    void Update(Size finalSize)
    {
        // v2p6.1 - lost access to this, always false now (probably the case anyway)
        _backendSupportsIndividualCorners = false;
        //??= AvaloniaLocator.Current.GetRequiredService<IPlatformRenderInterface>()
        //    .SupportsIndividualRoundRects;
        _size = finalSize;

        _initialized = true;

        if (BorderThickness.IsUniform && (CornerRadius.IsUniform || _backendSupportsIndividualCorners == true))
        {
            _complexRender = null;
        }
        else
        {
            if (_complexRender != null)
            {
                _complexRender.Update(finalSize, BorderThickness, CornerRadius, BackgroundSizing);
            }
            else
            {
                _complexRender = new ComplexBorderRender(finalSize, BorderThickness, CornerRadius, BackgroundSizing);
            }
        }
    }

    public void Render(DrawingContext context, Size finalSize)
    {
        if (_size != finalSize || !_initialized)
            Update(finalSize);

        RenderCore(context);
    }

    void RenderCore(DrawingContext context)
    {
        if (_complexRender != null)
        {
            _complexRender.Render(context, Background, BorderBrush);
        }
        else
        {
            var borderThickness = _borderThickness.Top;
            IPen pen = null;

            if (BorderBrush != null && borderThickness > 0)
            {
                pen = new ImmutablePen(BorderBrush.ToImmutable(), borderThickness);
            }

            var rect = new Rect(_size);

            if (_backgroundSizing == BackgroundSizing.InnerBorderEdge)
            {
                if (!MathUtilities.IsZero(borderThickness))
                {
                    rect = rect.Deflate(borderThickness);
                }
                var rrect = new RoundedRect(rect, _cornerRadius.TopLeft, _cornerRadius.TopRight,
                    _cornerRadius.BottomRight, _cornerRadius.BottomLeft);

                context.DrawRectangle(Background, null, rrect, BoxShadow);

                rrect = new RoundedRect(new Rect(_size).Deflate(borderThickness / 2), _cornerRadius.TopLeft, _cornerRadius.TopRight,
                    _cornerRadius.BottomRight, _cornerRadius.BottomLeft);

                context.DrawRectangle(null, pen, rrect, BoxShadow);
            }
            else
            {
                var rrect = new RoundedRect(rect, _cornerRadius.TopLeft, _cornerRadius.TopRight,
                    _cornerRadius.BottomRight, _cornerRadius.BottomLeft);

                context.DrawRectangle(Background, null, rrect, BoxShadow);

                rrect = new RoundedRect(new Rect(_size).Deflate(borderThickness / 2), _cornerRadius.TopLeft, _cornerRadius.TopRight,
                    _cornerRadius.BottomRight, _cornerRadius.BottomLeft);

                context.DrawRectangle(null, pen, rrect, BoxShadow);
            }
        }
    }

    private static void CreateGeometry(StreamGeometryContext context, Rect boundRect, BorderGeometryKeypoints keypoints)
    {
        context.BeginFigure(keypoints.TopLeft, true);

        // Top
        context.LineTo(keypoints.TopRight);

        // TopRight corner
        var radiusX = boundRect.TopRight.X - keypoints.TopRight.X;
        var radiusY = keypoints.RightTop.Y - boundRect.TopRight.Y;
        if (radiusX != 0 || radiusY != 0)
        {
            context.ArcTo(keypoints.RightTop, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise);
        }

        // Right
        context.LineTo(keypoints.RightBottom);

        // BottomRight corner
        radiusX = boundRect.BottomRight.X - keypoints.BottomRight.X;
        radiusY = boundRect.BottomRight.Y - keypoints.RightBottom.Y;
        if (radiusX != 0 || radiusY != 0)
        {
            context.ArcTo(keypoints.BottomRight, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise);
        }

        // Bottom
        context.LineTo(keypoints.BottomLeft);

        // BottomLeft corner
        radiusX = keypoints.BottomLeft.X - boundRect.BottomLeft.X;
        radiusY = boundRect.BottomLeft.Y - keypoints.LeftBottom.Y;
        if (radiusX != 0 || radiusY != 0)
        {
            context.ArcTo(keypoints.LeftBottom, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise);
        }

        // Left
        context.LineTo(keypoints.LeftTop);

        // TopLeft corner
        radiusX = keypoints.TopLeft.X - boundRect.TopLeft.X;
        radiusY = keypoints.LeftTop.Y - boundRect.TopLeft.Y;

        if (radiusX != 0 || radiusY != 0)
        {
            context.ArcTo(keypoints.TopLeft, new Size(radiusX, radiusY), 0, false, SweepDirection.Clockwise);
        }

        context.EndFigure(true);
    }

    private bool? _backendSupportsIndividualCorners;
    private Size _size;
    private Thickness _borderThickness;
    private CornerRadius _cornerRadius;
    private bool _initialized;
    private BackgroundSizing _backgroundSizing;
    private ComplexBorderRender _complexRender;

    private readonly struct BorderGeometryKeypoints
    {
        internal BorderGeometryKeypoints(Rect boundRect, Thickness borderThickness, CornerRadius cornerRadius,
            BackgroundSizing backgroundSizing, bool inner)
        {
            double leftTopY;
            double topLeftX;
            double topRightX;
            double rightTopY;
            double rightBottomY;
            double bottomRightX;
            double bottomLeftX;
            double leftBottomY;

            if (inner)
            {
                if (backgroundSizing == BackgroundSizing.InnerBorderEdge)
                {
                    var left = 0.5 * borderThickness.Left;
                    var top = 0.5 * borderThickness.Top;
                    var right = 0.5 * borderThickness.Right;
                    var bottom = 0.5 * borderThickness.Bottom;

                    // The default from the copied code is InnerBorderEdge
                    leftTopY = Math.Max(0, cornerRadius.TopLeft - top) + boundRect.TopLeft.Y;
                    topLeftX = Math.Max(0, cornerRadius.TopLeft - left) + boundRect.TopLeft.X;
                    topRightX = boundRect.Width - Math.Max(0, cornerRadius.TopRight - top) + boundRect.TopLeft.X;
                    rightTopY = Math.Max(0, cornerRadius.TopRight - right) + boundRect.TopLeft.Y;
                    rightBottomY = boundRect.Height - Math.Max(0, cornerRadius.BottomRight - bottom) +
                                   boundRect.TopLeft.Y;
                    bottomRightX = boundRect.Width - Math.Max(0, cornerRadius.BottomRight - right) +
                                   boundRect.TopLeft.X;
                    bottomLeftX = Math.Max(0, cornerRadius.BottomLeft - left) + boundRect.TopLeft.X;
                    leftBottomY = boundRect.Height - Math.Max(0, cornerRadius.BottomLeft - bottom) +
                                  boundRect.TopLeft.Y;
                }
                else
                {
                    var left = 0.5 * borderThickness.Left;
                    var top = 0.5 * borderThickness.Top;
                    var right = 0.5 * borderThickness.Right;
                    var bottom = 0.5 * borderThickness.Bottom;

                    leftTopY = Math.Max(0, cornerRadius.TopLeft - top) + boundRect.TopLeft.Y;
                    topLeftX = Math.Max(0, cornerRadius.TopLeft - left) + boundRect.TopLeft.X;
                    topRightX = boundRect.Width - Math.Max(0, cornerRadius.TopRight - top) + boundRect.TopLeft.X;
                    rightTopY = Math.Max(0, cornerRadius.TopRight - right) + boundRect.TopLeft.Y;
                    rightBottomY = boundRect.Height - Math.Max(0, cornerRadius.BottomRight - bottom) +
                                   boundRect.TopLeft.Y;
                    bottomRightX = boundRect.Width - Math.Max(0, cornerRadius.BottomRight - right) +
                                   boundRect.TopLeft.X;
                    bottomLeftX = Math.Max(0, cornerRadius.BottomLeft - left) + boundRect.TopLeft.X;
                    leftBottomY = boundRect.Height - Math.Max(0, cornerRadius.BottomLeft - bottom) +
                                  boundRect.TopLeft.Y;
                }
            }
            else
            {
                if (backgroundSizing == BackgroundSizing.InnerBorderEdge)
                {
                    var left = 0.5 * borderThickness.Left;
                    var top = 0.5 * borderThickness.Top;
                    var right = 0.5 * borderThickness.Right;
                    var bottom = 0.5 * borderThickness.Bottom;

                    leftTopY = cornerRadius.TopLeft + top + boundRect.TopLeft.Y;
                    topLeftX = cornerRadius.TopLeft + left + boundRect.TopLeft.X;
                    topRightX = boundRect.Width - (cornerRadius.TopRight + right) + boundRect.TopLeft.X;
                    rightTopY = cornerRadius.TopRight + top + boundRect.TopLeft.Y;
                    rightBottomY = boundRect.Height - (cornerRadius.BottomRight + bottom) + boundRect.TopLeft.Y;
                    bottomRightX = boundRect.Width - (cornerRadius.BottomRight + right) + boundRect.TopLeft.X;
                    bottomLeftX = cornerRadius.BottomLeft + left + boundRect.TopLeft.X;
                    leftBottomY = boundRect.Height - (cornerRadius.BottomLeft + bottom) + boundRect.TopLeft.Y;
                }
                else
                {
                    var left = 0.5 * borderThickness.Left;
                    var top = 0.5 * borderThickness.Top;
                    var right = 0.5 * borderThickness.Right;
                    var bottom = 0.5 * borderThickness.Bottom;

                    leftTopY = cornerRadius.TopLeft + top + boundRect.TopLeft.Y;
                    topLeftX = cornerRadius.TopLeft + left + boundRect.TopLeft.X;
                    topRightX = boundRect.Width - (cornerRadius.TopRight + right) + boundRect.TopLeft.X;
                    rightTopY = cornerRadius.TopRight + top + boundRect.TopLeft.Y;
                    rightBottomY = boundRect.Height - (cornerRadius.BottomRight + bottom) + boundRect.TopLeft.Y;
                    bottomRightX = boundRect.Width - (cornerRadius.BottomRight + right) + boundRect.TopLeft.X;
                    bottomLeftX = cornerRadius.BottomLeft + left + boundRect.TopLeft.X;
                    leftBottomY = boundRect.Height - (cornerRadius.BottomLeft + bottom) + boundRect.TopLeft.Y;
                }
            }

            var leftTopX = boundRect.TopLeft.X;
            var topLeftY = boundRect.TopLeft.Y;
            var topRightY = boundRect.TopLeft.Y;
            var rightTopX = boundRect.Width + boundRect.TopLeft.X;
            var rightBottomX = boundRect.Width + boundRect.TopLeft.X;
            var bottomRightY = boundRect.Height + boundRect.TopLeft.Y;
            var bottomLeftY = boundRect.Height + boundRect.TopLeft.Y;
            var leftBottomX = boundRect.TopLeft.X;

            LeftTop = new Point(leftTopX, leftTopY);
            TopLeft = new Point(topLeftX, topLeftY);
            TopRight = new Point(topRightX, topRightY);
            RightTop = new Point(rightTopX, rightTopY);
            RightBottom = new Point(rightBottomX, rightBottomY);
            BottomRight = new Point(bottomRightX, bottomRightY);
            BottomLeft = new Point(bottomLeftX, bottomLeftY);
            LeftBottom = new Point(leftBottomX, leftBottomY);

            // Fix overlap
            if (TopLeft.X > TopRight.X)
            {
                var scaledX = topLeftX / (topLeftX + topRightX) * boundRect.Width;
                TopLeft = new Point(scaledX, TopLeft.Y);
                TopRight = new Point(scaledX, TopRight.Y);
            }

            if (RightTop.Y > RightBottom.Y)
            {
                var scaledY = rightBottomY / (rightTopY + rightBottomY) * boundRect.Height;
                RightTop = new Point(RightTop.X, scaledY);
                RightBottom = new Point(RightBottom.X, scaledY);
            }

            if (BottomRight.X < BottomLeft.X)
            {
                var scaledX = bottomLeftX / (bottomLeftX + bottomRightX) * boundRect.Width;
                BottomRight = new Point(scaledX, BottomRight.Y);
                BottomLeft = new Point(scaledX, BottomLeft.Y);
            }

            if (LeftBottom.Y < LeftTop.Y)
            {
                var scaledY = leftTopY / (leftTopY + leftBottomY) * boundRect.Height;
                LeftBottom = new Point(LeftBottom.X, scaledY);
                LeftTop = new Point(LeftTop.X, scaledY);
            }
        }

        internal Point LeftTop { get; }

        internal Point TopLeft { get; }

        internal Point TopRight { get; }

        internal Point RightTop { get; }

        internal Point RightBottom { get; }

        internal Point BottomRight { get; }

        internal Point BottomLeft { get; }

        internal Point LeftBottom { get; }
    }

    private class ComplexBorderRender
    {
        public ComplexBorderRender(Size finalSize, Thickness borderThickness, CornerRadius cornerRadius, BackgroundSizing backgroundSizing)
        {
            CreateGeometry(finalSize, borderThickness, cornerRadius, backgroundSizing);
        }

        public void Update(Size finalSize, Thickness borderThickness, CornerRadius cornerRadius, BackgroundSizing backgroundSizing)
        {
            CreateGeometry(finalSize, borderThickness, cornerRadius, backgroundSizing);
        }

        public void Render(DrawingContext context, IBrush background, IBrush borderBrush)
        {
            if (_backgroundGeometry != null)
            {
                context.DrawGeometry(background, null, _backgroundGeometry);
            }

            if (_borderGeometry != null)
            {
                context.DrawGeometry(borderBrush, null, _borderGeometry);
            }
        }

        private void CreateGeometry(Size finalSize, Thickness borderThickness, CornerRadius cornerRadius, BackgroundSizing backgroundSizing)
        {
            BorderGeometryKeypoints backgroundKeypoints = default;
            StreamGeometry backgroundGeometry = null;

            if (backgroundSizing == BackgroundSizing.InnerBorderEdge)
            {
                var boundRect = new Rect(finalSize);
                var innerRect = boundRect.Deflate(borderThickness);

                if (innerRect.Width != 0 && innerRect.Height != 0)
                {
                    backgroundGeometry = new StreamGeometry();
                    backgroundKeypoints = new BorderGeometryKeypoints(innerRect, borderThickness, cornerRadius,
                        backgroundSizing, true);

                    using (var ctx = backgroundGeometry.Open())
                    {
                        BorderRenderHelper.CreateGeometry(ctx, innerRect, backgroundKeypoints);
                    }

                    _backgroundGeometry = backgroundGeometry;
                }
                else
                {
                    _backgroundGeometry = null;
                }

                if (boundRect.Width != 0 && boundRect.Height != 0)
                {
                    innerRect = boundRect.Deflate(borderThickness);
                    var borderGeometryKeypoints =
                        new BorderGeometryKeypoints(boundRect, borderThickness, cornerRadius, backgroundSizing, false);
                    var borderGeometry = new StreamGeometry();

                    using (var ctx = borderGeometry.Open())
                    {
                        BorderRenderHelper.CreateGeometry(ctx, innerRect, backgroundKeypoints!);

                        if (backgroundGeometry != null)
                        {
                            BorderRenderHelper.CreateGeometry(ctx, boundRect, borderGeometryKeypoints);
                        }
                    }

                    _borderGeometry = borderGeometry;
                }
                else
                {
                    _borderGeometry = null;
                }
            }
            else
            {
                var boundRect = new Rect(finalSize);
                var innerRect = boundRect.Deflate(borderThickness);

                if (innerRect.Width != 0 && innerRect.Height != 0)
                {
                    backgroundGeometry = new StreamGeometry();
                    backgroundKeypoints = new BorderGeometryKeypoints(boundRect, borderThickness, cornerRadius,
                        backgroundSizing, false);

                    using (var ctx = backgroundGeometry.Open())
                    {
                        BorderRenderHelper.CreateGeometry(ctx, boundRect, backgroundKeypoints);
                    }

                    _backgroundGeometry = backgroundGeometry;
                }
                else
                {
                    _backgroundGeometry = null;
                }

                if (boundRect.Width != 0 && boundRect.Height != 0)
                {
                    var borderGeometryKeypoints =
                        new BorderGeometryKeypoints(innerRect, borderThickness, cornerRadius, backgroundSizing, false);
                    var borderGeometry = new StreamGeometry();

                    using (var ctx = borderGeometry.Open())
                    {
                        BorderRenderHelper.CreateGeometry(ctx, innerRect, borderGeometryKeypoints);

                        if (backgroundGeometry != null)
                        {
                            BorderRenderHelper.CreateGeometry(ctx, boundRect, backgroundKeypoints);
                        }
                    }

                    _borderGeometry = borderGeometry;
                }
                else
                {
                    _borderGeometry = null;
                }
            }
        }

        private StreamGeometry _backgroundGeometry;
        private StreamGeometry _borderGeometry;
    }
}

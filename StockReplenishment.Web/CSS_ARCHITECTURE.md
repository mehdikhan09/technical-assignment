# CSS Architecture - METS Stock Replenishment System

This document outlines the CSS architecture and styling guidelines for the Stock Replenishment System.

## 📁 File Structure

```
StockReplenishment.Web/wwwroot/css/
├── app.css          # Main application styles (600+ lines)
└── site.css         # Additional site-specific styles (300+ lines)
```

## 🎨 Design System

### Color Palette

```css
Primary:     #1976d2  (Blue)
Secondary:   #424242  (Dark Gray)
Success:     #2e7d32  (Green)
Warning:     #ed6c02  (Orange)
Error:       #d32f2f  (Red)
Info:        #0288d1  (Light Blue)
Background:  #f5f5f5  (Light Gray)
Surface:     #ffffff  (White)
```

### Typography

- **Font Family**: Roboto, Segoe UI, Tahoma, Geneva, Verdana, sans-serif
- **Font Weights**: 
  - Light: 300
  - Regular: 400
  - Medium: 500
  - Bold: 700

### Spacing Scale

- **2**: 8px
- **3**: 16px
- **4**: 24px
- **6**: 48px

### Elevation (Shadows)

- **sm**: `0 2px 4px rgba(0,0,0,0.1)`
- **md**: `0 4px 8px rgba(0,0,0,0.12)`
- **lg**: `0 8px 16px rgba(0,0,0,0.15)`

### Border Radius

- **Default**: 8px
- **Chips/Pills**: 16px
- **Circular**: 50%

## 🎯 Component Styles

### Dashboard Components

#### Summary Cards (`.stat-card`)
- Gradient background
- Color-coded left border
- Large number display (2.5rem)
- Hover elevation effect

**Variants**:
- `.stat-card.success` - Green border
- `.stat-card.warning` - Orange border
- `.stat-card.info` - Blue border
- `.stat-card.error` - Red border

#### Dashboard Sections (`.dashboard-section`)
- Consistent spacing (32px margin-bottom)
- Section titles with icons
- Fade-in animation

### Status & Priority Chips

#### Status Colors
- **Draft**: Gray (#9e9e9e)
- **Submitted**: Blue (#2196f3)
- **Approved**: Green (#4caf50)
- **Rejected**: Red (#f44336)
- **Fulfilled**: Purple (#9c27b0)

#### Priority Colors
- **Low**: Green (#66bb6a)
- **Normal**: Orange (#ff9800)
- **Urgent**: Red (#f44336)

### Tables

- Hover effect on rows
- Dense padding option
- Sticky headers support
- Responsive design
- Shadow on hover

### Forms

#### Form Sections (`.form-section`)
- White background
- Consistent padding (24px)
- Shadow elevation
- Border radius (8px)

#### Line Items (`.line-item-card`)
- Light gray background (#fafafa)
- Left color-coded border
- Numbered badges
- Delete button

### Buttons

#### Primary Actions
- Filled variant
- Box shadow
- Hover lift effect (-1px translateY)

#### Secondary Actions
- Outlined variant
- Subtle hover background
- Color-coded by context

#### Quick Actions (`.quick-action-btn`)
- Large size
- Transform on hover (-2px translateY)
- Elevated shadow

## 🎭 Animations

### Available Animations

1. **fadeIn**: Opacity 0→1, translateY 10px→0
2. **slideIn**: Opacity 0→1, translateX -20px→0
3. **pulse**: Opacity oscillation (1→0.6→1)
4. **loading**: Gradient background shift

### Usage

```html
<div class="fade-in">Content</div>
<div class="slide-in">Content</div>
<div class="pulse">Loading...</div>
```

## 📱 Responsive Breakpoints

```css
Mobile:    max-width: 600px
Tablet:    max-width: 960px
Desktop:   min-width: 961px
```

### Mobile Adaptations

- Stat cards: Reduced font size (2rem → 1.75rem)
- Tables: Reduced padding (8px), smaller font (0.875rem)
- Filters: Stack vertically, full width
- Quick actions: Full width buttons, vertical layout
- Action buttons: Full width

## 🌙 Dark Mode Support

Automatically adapts based on system preference:

```css
@media (prefers-color-scheme: dark) {
  --background-color: #121212
  --surface-color: #1e1e1e
  --text-primary: rgba(255, 255, 255, 0.87)
  --text-secondary: rgba(255, 255, 255, 0.6)
}
```

## 🖨️ Print Styles

Print-optimized styles hide:
- Navigation bars
- Action buttons
- Quick actions
- Filters

And adjust:
- Remove shadows
- Add borders for clarity
- Prevent page breaks inside cards
- White background

## ♿ Accessibility Features

### Focus States
- 2px solid outline in primary color
- 2px offset for visibility
- Applied to all interactive elements

### Screen Reader Support
- `.sr-only` class for screen reader-only content
- Proper semantic HTML structure
- ARIA labels where needed

### Motion Preferences
```css
@media (prefers-reduced-motion: reduce) {
  /* Animations disabled or minimal */
}
```

### High Contrast
- Sufficient color contrast ratios
- Border indicators for states
- Icon + text combinations

## 🔧 Utility Classes

### Display & Layout
```css
.d-flex              /* display: flex */
.justify-space-between
.justify-center
.align-center
.text-center
.text-right
```

### Spacing
```css
.mt-2, .mt-3, .mt-4, .mt-6  /* margin-top */
.mb-2, .mb-3, .mb-4, .mb-6  /* margin-bottom */
.pa-2, .pa-3, .pa-4, .pa-6  /* padding (all sides) */
.gap-2, .gap-3, .gap-4       /* flex gap */
```

### Elevation
```css
.elevation-1  /* shadow-sm */
.elevation-2  /* shadow-md */
.elevation-3  /* shadow-lg */
```

## 🎨 MudBlazor Integration

The custom CSS works alongside MudBlazor's built-in styles:

1. **MudBlazor base styles** (from CDN)
2. **Bootstrap** (minimal, for grid only)
3. **app.css** (main custom styles)
4. **site.css** (additional enhancements)
5. **Component-scoped CSS** (Blazor isolated styles)

## 📝 Best Practices

### Do's ✅
- Use CSS variables for consistent theming
- Apply utility classes for spacing
- Use MudBlazor components as base
- Add custom classes for enhancement
- Test in mobile, tablet, desktop
- Verify dark mode appearance
- Check print preview

### Don'ts ❌
- Don't use inline styles
- Don't hardcode colors (use variables)
- Don't create conflicting MudBlazor overrides
- Don't forget hover/focus states
- Don't ignore accessibility
- Don't skip responsive testing

## 🚀 Performance

### CSS Optimization
- Minimal specificity (single classes)
- No deep nesting (max 2-3 levels)
- Efficient selectors
- CSS variables for runtime theming
- Compressed for production

### Loading Strategy
1. Critical CSS inline (future enhancement)
2. MudBlazor from CDN (cached)
3. Custom CSS bundled and minified
4. Component CSS lazy-loaded

## 🔄 Future Enhancements

1. **CSS-in-JS**: Consider styled components for dynamic theming
2. **Design Tokens**: Extract all variables to JSON config
3. **Theme Switcher**: Manual dark/light mode toggle
4. **Custom Themes**: Allow users to customize colors
5. **Component Library**: Storybook for style documentation
6. **CSS Grid**: Replace some flex layouts with grid
7. **Container Queries**: More responsive components
8. **CSS Layers**: Better cascade control

## 📚 References

- [MudBlazor Documentation](https://mudblazor.com/)
- [Material Design Guidelines](https://material.io/design)
- [CSS Variables Guide](https://developer.mozilla.org/en-US/docs/Web/CSS/Using_CSS_custom_properties)
- [Web Accessibility Initiative](https://www.w3.org/WAI/)

## 🤝 Contributing

When adding new styles:

1. Check if MudBlazor has a built-in solution
2. Use existing CSS variables
3. Follow BEM naming convention where applicable
4. Add responsive breakpoints
5. Test dark mode
6. Document in this README
7. Add comments in CSS file

---

**Last Updated**: 2025
**Maintained By**: Development Team

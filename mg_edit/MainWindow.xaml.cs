using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using mg_edit.TextEdit;
using System.Windows.Media;
using System.Windows.Input;

namespace mg_edit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Resolution of drawn lines
        private const int LINE_RESOLUTION = 5;

        // Seek length of minor scroll
        private const double MINOR_SCROLL_WIDTH = 500;

        // Strength of scroll
        private const int SCROLL_TICK = 20;

        // Side window used as text editor
        private TextEditWindow textEditWindow;

        // Size of cursor
        private const double CURSOR_RADIUS = 6;

        // Marker for cursor
        private Ellipse cursorMarker = null;

        // List of guidelines
        private List<Line> guideLines = new List<Line>();
        
        // List of entities being drawn
        private HashSet<Entity> drawnEntities = new HashSet<Entity>();

        // Translates a point in game space to canvas space
        private (double, double) TranslateToCanvasSpace((double, double) pos)
        {
            // Applies a linear mapping from gamespace to canvas space
            return (
                    (CenterCanvas.Width * pos.Item1) / (2 * (GameState.GAMESPACE_PADDING + GameState.GAMESPACE_WIDTH)) + CenterCanvas.Width / 2,
                    (CenterCanvas.Height * -pos.Item2) / (2 * (GameState.GAMESPACE_PADDING + GameState.GAMESPACE_HEIGHT)) + CenterCanvas.Height / 2
                );
        }

        // Translates a point in canvas space to game space
        private (double, double) TranslateToGameSpace((double, double) pos)
        {
            // Applies a linear mapping from canvas space to gamespace
            return (
                    (2 * (GameState.GAMESPACE_PADDING + GameState.GAMESPACE_WIDTH)) * (pos.Item1 - CenterCanvas.Width / 2) / CenterCanvas.Width,
                    -(2 * (GameState.GAMESPACE_PADDING + GameState.GAMESPACE_HEIGHT)) * (pos.Item2 - CenterCanvas.Height / 2) / CenterCanvas.Height
                );
        }

        // Draws a line on the canvas, takes game space coordinates
        public Line DrawLine(double x1, double y1, double x2, double y2)
        {
            var line = new Line();
            line.Stroke = System.Windows.Media.Brushes.Black;
            (line.X1, line.Y1) = this.TranslateToCanvasSpace((x1, y1));
            (line.X2, line.Y2) = this.TranslateToCanvasSpace((x2, y2));
            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.StrokeThickness = 2;
            CenterCanvas.Children.Add(line);
            return line;
        }

        // Sets up canvas to have correct guidelines
        public void DrawGuideLines()
        {
            foreach (var line in guideLines)
            {
                CenterCanvas.Children.Remove(line);
            }

            guideLines.Clear();

            // Guidelines form a rectangle with each corner
            guideLines.Add(this.DrawLine(GameState.GAMESPACE_WIDTH, GameState.GAMESPACE_HEIGHT, GameState.GAMESPACE_WIDTH, -GameState.GAMESPACE_HEIGHT));
            guideLines.Add(this.DrawLine(GameState.GAMESPACE_WIDTH, -GameState.GAMESPACE_HEIGHT, -GameState.GAMESPACE_WIDTH, -GameState.GAMESPACE_HEIGHT));
            guideLines.Add(this.DrawLine(-GameState.GAMESPACE_WIDTH, -GameState.GAMESPACE_HEIGHT, -GameState.GAMESPACE_WIDTH, GameState.GAMESPACE_HEIGHT));
            guideLines.Add(this.DrawLine(-GameState.GAMESPACE_WIDTH, GameState.GAMESPACE_HEIGHT, GameState.GAMESPACE_WIDTH, GameState.GAMESPACE_HEIGHT));

        }

        // Removes an entities lines, but does not fully undraw
        private void RemoveEntityLines(Entity entity)
        {
            entity.GetLines().ForEach(line => CenterCanvas.Children.Remove(line));
            entity.ClearLines();
            // Remove marker
            CenterCanvas.Children.Remove(entity.GetMarker());
        }

        // Takes an entity that is drawn and undraws it
        private void UndrawEntity(Entity entity)
        {
            drawnEntities.Remove(entity);
            RemoveEntityLines(entity);
        }

        // Draw a single entity
        private void DrawEntity(Entity entity)
        {
            drawnEntities.Add(entity);
            var positions = entity.GetPositions();
            // Draw from last position to current
            // Add each line segment
            for (int i = 1; i < positions.Count; i += LINE_RESOLUTION)
            {
                int a = Math.Max(0, i - LINE_RESOLUTION);
                int b = i;
                entity.AddLine(
                    this.DrawLine(
                        positions[a].Item1, positions[a].Item2, 
                        positions[b].Item1, positions[b].Item2
                    )
                );
            }
            // Add marker
            CenterCanvas.Children.Add(entity.GetMarker());
        }

        // Draw all active entities
        public void DrawAllActiveEntities()
        {
            var activeEntities = GameState.Get().GetActiveEntities();
            activeEntities.ForEach(entity => this.DrawEntity(entity));
        }

        // Undraws all entities currently drawn
        public void UndrawAllEntities()
        {
            foreach (Entity ent in drawnEntities)
            {
                this.RemoveEntityLines(ent);
            }
            drawnEntities.Clear();
        }

        // Redraws active entity list box
        public void RedrawActiveList()
        {
            textEditWindow.ActiveListBox.Items.Clear();
            GameState.Get().GetActiveEntities().ForEach(
                ent => {
                    textEditWindow.ActiveListBox.Items.Add(ent);
                }
            );

        }

        // Computes difference in current entities, 
        // Deletes stuff thats not drawn, draws new stuff
        // Forces redraw of lines when true
        public void UpdateEntityView(bool force = false)
        {
            HashSet<Entity> toDraw = new HashSet<Entity>(GameState.Get().GetActiveEntities());
            HashSet<Entity> toRemove = new HashSet<Entity>(drawnEntities);

            //Compute entities that need to be drawn
            toDraw.ExceptWith(drawnEntities);
            toRemove.ExceptWith(new HashSet<Entity>(GameState.Get().GetActiveEntities()));

            // Update view
            toDraw.ToList().ForEach(entity => DrawEntity(entity));
            toRemove.ToList().ForEach(entity => UndrawEntity(entity));

            // Redraw list box is an entity passed into/out of view
            if (toDraw.Count + toRemove.Count > 0)
            {
                RedrawActiveList();
            }

            // Redraw lines if forced
            if (force)
            {
                GameState.Get().GetActiveEntities().ForEach( ent => { UndrawEntity(ent); DrawEntity(ent); } );
            }

            // Update markers
            int tick = GameState.Get().Tick;
            GameState.Get().GetActiveEntities().ForEach(
                ent => {
                    (double, double) pos = this.TranslateToCanvasSpace(ent.GetPosition(tick));
                    ent.SetMarkerPosition(pos);
                }
            );
        }

        // Draw marker for user click
        // Returns position in game space
        public (double, double) SetMarker((double, double) position)
        {
            if (cursorMarker is null)
            {
                cursorMarker = new Ellipse()
                {
                    Width = 2 * CURSOR_RADIUS,
                    Height = 2 * CURSOR_RADIUS,
                    Stroke = Brushes.Green,
                    StrokeThickness = 2
                };
                CenterCanvas.Children.Add(cursorMarker);
            }

            cursorMarker.Margin = new System.Windows.Thickness(
                    position.Item1 - CURSOR_RADIUS,
                    position.Item2 - CURSOR_RADIUS,
                    0,
                    0
                );

            position = TranslateToGameSpace(position);
            position = ((int)position.Item1, (int)position.Item2);

            CursorLabel.Content = "(" + position.Item1.ToString() + ", " + position.Item2.ToString() + ")";
        
            // Set gamestate's position
            GameState.Get().CursorPosition = position;

            return position;
        }

        // Event handler for canvas click
        public void UpdateCanvasCursorClick(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(CenterCanvas);
            SetMarker((pos.X, pos.Y));
        }

        // Updates tick and redraws
        public void UpdateScroll(object sender, RoutedEventArgs e)
        {
            double major = LevelMasterScroll.Value * GameState.Get().GetLevelTotalLength();
            double minor = LevelMinorScroll.Value * MINOR_SCROLL_WIDTH;
            GameState.Get().Tick = (int)(major + minor);
            TickLabel.Content = GameState.Get().Tick;

            UpdateEntityView();
        }

        // Resets scroll so that the right shuffle is centered
        public void ResetShuffle(object sender, RoutedEventArgs e)
        {
            int Tick = GameState.Get().Tick;
            // Work out how right shuffle should be
            double minor = MINOR_SCROLL_WIDTH / 2;

            if (Tick < minor)
            {
                minor = Tick;
            }

            double major = Tick - minor;

            LevelMasterScroll.Value = major / GameState.Get().GetLevelTotalLength();
            LevelMinorScroll.Value = minor / MINOR_SCROLL_WIDTH;
        }

        // Handles mouse wheel scrolling
        public void MouseWheelUpdate(object sender, MouseWheelEventArgs e)
        {
            GameState.Get().Tick -= SCROLL_TICK * e.Delta / Math.Abs(e.Delta);
            GameState.Get().Tick = Math.Max(GameState.Get().Tick, 0);
            GameState.Get().Tick = Math.Min(GameState.Get().Tick, GameState.GetLevel().GetLevelLength());
            TickLabel.Content = GameState.Get().Tick;

            UpdateEntityView();

            ResetShuffle(null, null);
        }

        // Loads level from GameState's load table
        public void LoadLevel()
        {
            // Load from default
            GameState.Get().LoadLevel();

            // Draw level
            this.UpdateScroll(null, null);

            // Update sidebar with level
            this.textEditWindow.UpdateText(GameState.Get().LevelFolder);
        }

        // Reloads level current loadables
        public void ReloadLevel()
        {
            // Load from string
            GameState.Get().ReloadLevel();

            // Draw level
            this.UpdateScroll(null, null);
        }

        // Handler for when this window is closed
        void HandleClose(object sender, CancelEventArgs e)
        {
            // Close side window
            this.textEditWindow.Close();
        }

        // Prepares the base window
        public MainWindow()
        {
            // Initialise application
            GameState.Get().MainWindow = this;

            InitializeComponent();

            // Set up guidelines
            this.DrawGuideLines();

            // Draw all entities that are currently active and visible
            this.UpdateEntityView();

            // Create side text editor
            this.Show();
            this.textEditWindow = new TextEditWindow(this);
            GameState.Get().TextEditWindow = textEditWindow;
            this.textEditWindow.Show();
        }
    }
}

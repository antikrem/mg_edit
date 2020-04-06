using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mg_edit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // List of guidelines
        private List<Line> guideLines = new List<Line>();
        
        // List of entities being drawn
        private HashSet<Entity> drawnEntities = new HashSet<Entity>();

        // Translates a point in game space to canvas space
        private (double, double) TranslateToCanvasSpace(double x, double y)
        {
            // Applies a linear mapping from gamespace to canvas space
            return (
                    (CenterCanvas.Width * x) / (2 * (GameState.GAMESPACE_PADDING + GameState.GAMESPACE_WIDTH)) + CenterCanvas.Width / 2,
                    (CenterCanvas.Height * y) / (2 * (GameState.GAMESPACE_PADDING + GameState.GAMESPACE_HEIGHT)) + CenterCanvas.Height / 2
                );
        }

        // Draws a line on the canvas, takes game space coordinates
        public Line DrawLine(double x1, double y1, double x2, double y2)
        {
            var line = new Line();
            line.Stroke = System.Windows.Media.Brushes.Black;
            (line.X1, line.Y1) = this.TranslateToCanvasSpace(x1, y1);
            (line.X2, line.Y2) = this.TranslateToCanvasSpace(x2, y2);
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
            for (int i = 1; i < positions.Count; i++)
            {
                this.DrawLine(positions[i - 1].Item1, positions[i - 1].Item2, positions[i].Item1, positions[i].Item2);
            }
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

        // Computes difference in current entities, 
        // Deletes stuff thats not drawn, draws new stuff
        public void UpdateEntityView()
        {
            HashSet<Entity> toDraw = new HashSet<Entity>(GameState.Get().GetActiveEntities());
            HashSet<Entity> toRemove = new HashSet<Entity>(drawnEntities);

            //Compute entities that need to be drawn
            toDraw.ExceptWith(drawnEntities);
            toRemove.ExceptWith(new HashSet<Entity>(GameState.Get().GetActiveEntities()));

            // Update view
            toDraw.ToList().ForEach(entity => DrawEntity(entity));
            toRemove.ToList().ForEach(entity => UndrawEntity(entity));
        }

        // Updates tick and redraws
        public void UpdateScroll(object sender, RoutedEventArgs e)
        {
            UpdateEntityView();
        }

        // Prepares the base window
        public MainWindow()
        {
            // Initialise application
            GameState.Get();

            InitializeComponent();

            // Set up guidelines
            this.DrawGuideLines();

            // Draw all entities that are currently active and visible
            this.UpdateEntityView();
        }
    }
}

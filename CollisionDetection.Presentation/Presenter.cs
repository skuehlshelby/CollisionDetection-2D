using CollisionDetection.Model;
using CollisionDetection.Model.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CollisionDetection.Presentation
{
    public sealed class Presenter : ISimulationPresenter
    {
        private readonly object syncLock = new();
        private readonly Queue<Action<FrameOptionsBuilder>> requests;
        private readonly IShapeFactory shapeFactory;
        private readonly IColorTracker colorTracker;
        private readonly ICollection<IShape> shapes;
        private IFrameOptions frameOptions;
        private IFrame frame;

        public Presenter(IDefaultProvider defaultProvider, ISimulationView view)
        {
            requests = new Queue<Action<FrameOptionsBuilder>>();
            shapes = new List<IShape>();

            shapeFactory = defaultProvider.GetDefault<IShapeFactory>(MonitoredProperty.ShapeFactory);
            colorTracker = defaultProvider.GetDefault<IColorTracker>(MonitoredProperty.ColorTracker);

            frameOptions = new FrameOptionsBuilder(defaultProvider)
                .WithWorldBounds(GetWorldBoundsFromView(view))
                .Build();

            SpawnOrRemoveShapes(defaultProvider.GetDefault<int>(MonitoredProperty.ShapeCount));

            frame = new Frame(shapes, frameOptions);

            view.PropertyChanged += OnPropertyChanged;
        }

        private static Bounds GetWorldBoundsFromView(ISimulationView view)
        {
            var bounds = (Size)view.QueryProperty(MonitoredProperty.WorldBounds);
            return new Bounds((0, 0), (bounds.Width, bounds.Height));
        }

        void ISimulationPresenter.DrawScene(IGraphics graphics)
        {
            ProcessUserRequests();

            lock (syncLock)
            {
                frame = new Frame(shapes, frameOptions);
            }

            foreach(var drawable in frame.GetScene())
            {
                drawable.Draw(graphics);
            }
        }

        private void ProcessUserRequests()
        {
            lock (syncLock)
            {
                if (requests.Any())
                {
                    var builder = new FrameOptionsBuilder(frameOptions);

                    while (requests.Count > 0)
                    {
                        requests.Dequeue().Invoke(builder);
                    }

                    frameOptions = builder.Build();
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            lock (syncLock)
            {
                if (MonitoredProperty.AverageRenderTimeVisibility == e.MonitoredProperty)
                    requests.Enqueue(b => b.WithAverageRenderTimeVisibility((bool)e.Value));
                else if (MonitoredProperty.BoundingVolumeVisibility == e.MonitoredProperty)
                    requests.Enqueue(b => b.WithBoundingVolumeVisibility((bool)e.Value));
                else if (MonitoredProperty.Paused == e.MonitoredProperty)
                    requests.Enqueue(b => b.WithPaused((bool)e.Value));
                else if (MonitoredProperty.SplitMethod == e.MonitoredProperty)
                    requests.Enqueue(b => b.WithSplitMethod(e.Value.ToString()));
                else if (MonitoredProperty.CollisionHandler == e.MonitoredProperty)
                    requests.Enqueue(b => b.WithCollisionHandler(e.Value.ToString()));
                else if (MonitoredProperty.FrameRate == e.MonitoredProperty)
                    requests.Enqueue(b => b.WithFrameDuration(TimeSpan.FromSeconds(1.0F / (int)e.Value)));
                else if (MonitoredProperty.ColorAdded == e.MonitoredProperty)
                    colorTracker.Add((Color)e.Value);
                else if (MonitoredProperty.ColorRemoved == e.MonitoredProperty)
                    colorTracker.Remove((Color)e.Value);
                else if (MonitoredProperty.ShapeCount == e.MonitoredProperty)
                    SpawnOrRemoveShapes((int)e.Value);
                else if (MonitoredProperty.WorldBounds == e.MonitoredProperty)
                {
                    var size = (Size)e.Value;
                    requests.Enqueue(b => b.WithWorldBounds(new Bounds((0, 0), (size.Width, size.Height))));
                }
            }
        }

        private void SpawnOrRemoveShapes(int desiredNumberOfShapes)
        {
            while (shapes.Count > desiredNumberOfShapes)
            {
                //This logic should probably be inside something else
                Color color = colorTracker.GetMostCommonColorAndDecrementCount();
                Circle[] circles = shapes.OfType<Circle>().ToArray();

                shapes.Remove(circles.First(circle => circle.Color == color));
            }

            while (shapes.Count < desiredNumberOfShapes)
            {
                shapes.Add(shapeFactory.SpawnRandomly(frameOptions.GetWorldBounds()));
            }
        }
    }
}
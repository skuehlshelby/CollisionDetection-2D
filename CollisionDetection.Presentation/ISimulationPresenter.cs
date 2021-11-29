using System;
using CollisionDetection.Model.Application;
using CollisionDetection.Model.Rendering;
using CollisionDetection.Model.Rendering.Drawables;

namespace CollisionDetection.Presentation
{
    public interface ISimulationPresenter
    {

        void Update(TimeSpan timeStep);

        void Render(IGraphics graphics, params IDrawable[] extras);

        void Notify(Action<IApplication> change);
    }
}

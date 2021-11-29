using CollisionDetection.Model;
using CollisionDetection.Model.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using CollisionDetection.Model.Application;
using CollisionDetection.Model.Application.Commands;
using CollisionDetection.Model.Rendering.Drawables;

namespace CollisionDetection.Presentation
{
    public sealed class Presenter : ISimulationPresenter
    {
        private readonly object syncLock = new();
        private readonly Queue<Action<IApplication>> requests;
        private IApplication application;

        public Presenter(IDefaultProvider defaultProvider)
        {
            requests = new Queue<Action<IApplication>>();
        }

        void ISimulationPresenter.Update(TimeSpan timeStep)
        {
            ProcessUserRequests();

            lock (syncLock)
            {
                application.Update(timeStep);
            }
        }

        void ISimulationPresenter.Render(IGraphics graphics, params IDrawable[] extras)
        {
            lock (syncLock)
            {
                application.Render(graphics, extras);
            }
        }

        void ISimulationPresenter.Notify(Action<IApplication> change)
        {
            requests.Enqueue(change);
        }

        private void ProcessUserRequests()
        {
            lock (syncLock)
            {
                while (requests.Any())
                {
                    requests.Dequeue().Invoke(application);
                }
            }
        }
    }
}
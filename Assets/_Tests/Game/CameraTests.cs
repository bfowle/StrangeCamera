using System;
using System.Collections;
using UnityEngine;
using StrangeCamera.Game;
using strange.extensions.context.impl;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using strange.framework.api;
using strange.framework.impl;
using NUnit.Framework;

namespace StrangeCamera.UnitTests {

    [TestFixture]
    public class CameraTests {

        StubContext context;
        ICamera model;
        MockCameraView view;

        [SetUp]
        public void Init() {
            Context.firstContext = null;
            context = new StubContext(new object(), true);
            context.Start();

            // set mediation binding with stub
            context.injectionBinder.Bind<IMediationBinder>().To<StubMediationBinder>().ToSingleton();
            context.mediationBinder = context.injectionBinder
                .GetInstance<IMediationBinder>() as IMediationBinder;
            // set up model, mediator, view
            context.injectionBinder.Bind<ICamera>().To<CameraModel>().ToSingleton();
            model = context.injectionBinder.GetInstance<ICamera>() as ICamera;
            context.mediationBinder.Bind<MockCameraView>().To<StubCameraMediator>();
            view = new MockCameraView();
            // set up signals
            context.injectionBinder.Bind<CameraStateSignal>().ToSingleton();
            // finish setting up mediator
            context.AddView(view);
        }

        [Test]
        public void TestCameraStateSignalUpdatesState() {
            CameraStateSignal signal = context.injectionBinder
                .GetInstance<CameraStateSignal>() as CameraStateSignal;

            // test construction
            Assert.IsTrue(view.isInitted, "expected view to be initialized");

            // test cinematic state
            signal.Dispatch(CameraState.CINEMATIC);
            Assert.AreEqual(CameraState.CINEMATIC, model.state, "expected cinematic state to be set");
            Assert.IsTrue(view.isCinematic, "expected view to perform cinematic state operation");

            // test character state
            signal.Dispatch(CameraState.CHARACTER);
            Assert.AreEqual(CameraState.CHARACTER, model.state, "expected character state to be set");
            Assert.IsTrue(view.isCharacter, "expected view to perform character state operation");
        }

        [Test]
        public void TestCameraSequenceSignalFiresInOrderAndUpdatesState() {
            context.commandBinder.Bind<CameraSequenceSignal>().To<CameraFlythroughCommand>()
                .To<CameraAttachCommand>().InSequence();
            CameraSequenceSignal signal = context.injectionBinder
                .GetInstance<CameraSequenceSignal>() as CameraSequenceSignal;
            // get complete signal to fire manually between tests
            context.injectionBinder.Bind<FlythroughCompleteSignal>().ToSingleton();
            FlythroughCompleteSignal completeSignal = context.injectionBinder
                .GetInstance<FlythroughCompleteSignal>() as FlythroughCompleteSignal;

            // dispatch sequence signal
            signal.Dispatch();
            Assert.IsTrue(view.isCinematic, "expected flythrough command to be executed");

            // dispatch complete manually to continue the sequence
            completeSignal.Dispatch();
            Assert.IsTrue(view.isCharacter, "expected attach command to be executed");
        }
		
		//------------------------------
		//- test doubles
		//------------------------------

        class StubMediationBinder : Binder, IMediationBinder {

            [Inject]
            public IInjectionBinder injectionBinder { get; set; }

            public override IBinding GetRawBinding() {
                return new MediationBinding(resolver) as IBinding;
            }

            public void Trigger(MediationEvent evt, IView view) {
                StubCameraMediator mediator = new StubCameraMediator();
                injectionBinder.Bind<MockCameraView>().ToValue(view);
                injectionBinder.injector.Inject(mediator);
                injectionBinder.Unbind<MockCameraView>();
                mediator.OnRegister();
            }

        }

        class MockCameraView : MockView {

            public bool isInitted = false;
            public bool isCinematic = false;
            public bool isCharacter = false;

            internal void init() {
                isInitted = true;
            }

            internal void stateChanged(CameraState state) {
                if (state == CameraState.CINEMATIC) {
                    isCinematic = true;
                } else if (state == CameraState.CHARACTER) {
                    isCharacter = true;
                }
            }

        }

        class StubCameraMediator {

            [Inject]
            public ICamera model { get; set; }

            [Inject]
            public MockCameraView view { get; set; }

            [Inject]
            public CameraStateSignal cameraStateSignal { get; set; }

            public void OnRegister() {
                cameraStateSignal.AddListener(onCameraStateChanged);
                view.init();
            }

            void onCameraStateChanged(CameraState state) {
                model.SetState(state);
                view.stateChanged(state);
            }

        }

    }

}

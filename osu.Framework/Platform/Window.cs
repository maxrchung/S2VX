// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Extensions;
using osu.Framework.Input.StateChanges;
using osuTK;
using osuTK.Input;
using osuTK.Platform;

namespace osu.Framework.Platform
{
    /// <summary>
    /// Implementation of <see cref="IWindow"/> that provides bindables and
    /// delegates responsibility to window and graphics backends.
    /// </summary>
    public class Window : IWindow
    {
        private readonly IWindowBackend windowBackend;
        private readonly IGraphicsBackend graphicsBackend;

        #region Properties

        /// <summary>
        /// Gets and sets the window title.
        /// </summary>
        public string Title
        {
            get => windowBackend.Title;
            set => windowBackend.Title = value;
        }

        /// <summary>
        /// Enables or disables vertical sync.
        /// </summary>
        public bool VerticalSync
        {
            get => graphicsBackend.VerticalSync;
            set => graphicsBackend.VerticalSync = value;
        }

        /// <summary>
        /// Returns true if window has been created.
        /// Returns false if the window has not yet been created, or has been closed.
        /// </summary>
        public bool Exists => windowBackend.Exists;

        /// <summary>
        /// Returns the scale of window's drawable area.
        /// In high-dpi environments this will be greater than one.
        /// </summary>
        public float Scale => windowBackend.Scale;

        public Display PrimaryDisplay => windowBackend.PrimaryDisplay;

        public DisplayMode CurrentDisplayMode => windowBackend.CurrentDisplayMode;

        public IEnumerable<Display> Displays => windowBackend.Displays;

        public WindowMode DefaultWindowMode => Configuration.WindowMode.Windowed;

        #endregion

        #region Mutable Bindables

        /// <summary>
        /// Provides a bindable that controls the window's position.
        /// </summary>
        public Bindable<Point> Position { get; } = new Bindable<Point>();

        /// <summary>
        /// Provides a bindable that controls the window's unscaled internal size.
        /// </summary>
        public Bindable<Size> Size { get; } = new BindableSize();

        /// <summary>
        /// Provides a bindable that controls the window's <see cref="WindowState"/>.
        /// </summary>
        public Bindable<WindowState> WindowState { get; } = new Bindable<WindowState>();

        /// <summary>
        /// Provides a bindable that controls the window's <see cref="CursorState"/>.
        /// </summary>
        public Bindable<CursorState> CursorState { get; } = new Bindable<CursorState>();

        /// <summary>
        /// Provides a bindable that controls the window's visibility.
        /// </summary>
        public Bindable<bool> Visible { get; } = new BindableBool();

        public Bindable<Display> CurrentDisplay { get; } = new Bindable<Display>();

        public Bindable<WindowMode> WindowMode { get; } = new Bindable<WindowMode>();

        #endregion

        #region Immutable Bindables

        private readonly BindableBool isActive = new BindableBool(true);

        public IBindable<bool> IsActive => isActive;

        private readonly BindableBool focused = new BindableBool();

        /// <summary>
        /// Provides a read-only bindable that monitors the window's focused state.
        /// </summary>
        public IBindable<bool> Focused => focused;

        private readonly BindableBool cursorInWindow = new BindableBool();

        /// <summary>
        /// Provides a read-only bindable that monitors the whether the cursor is in the window.
        /// </summary>
        public IBindable<bool> CursorInWindow => cursorInWindow;

        public IBindableList<WindowMode> SupportedWindowModes { get; } = new BindableList<WindowMode>(Enum.GetValues(typeof(WindowMode)).OfType<WindowMode>());

        public BindableSafeArea SafeAreaPadding { get; } = new BindableSafeArea();

        #endregion

        #region Events

        /// <summary>
        /// Invoked once every window event loop.
        /// </summary>
        public event Action Update;

        /// <summary>
        /// Invoked after the window has resized.
        /// </summary>
        public event Action Resized;

        /// <summary>
        /// Invoked when the user attempts to close the window.
        /// </summary>
        public event Func<bool> ExitRequested;

        /// <summary>
        /// Invoked when the window is about to close.
        /// </summary>
        public event Action Exited;

        /// <summary>
        /// Invoked when the window loses focus.
        /// </summary>
        public event Action FocusLost;

        /// <summary>
        /// Invoked when the window gains focus.
        /// </summary>
        public event Action FocusGained;

        /// <summary>
        /// Invoked when the window becomes visible.
        /// </summary>
        public event Action Shown;

        /// <summary>
        /// Invoked when the window becomes invisible.
        /// </summary>
        public event Action Hidden;

        /// <summary>
        /// Invoked when the mouse cursor enters the window.
        /// </summary>
        public event Action MouseEntered;

        /// <summary>
        /// Invoked when the mouse cursor leaves the window.
        /// </summary>
        public event Action MouseLeft;

        /// <summary>
        /// Invoked when the window moves.
        /// </summary>
        public event Action<Point> Moved;

        /// <summary>
        /// Invoked when the user scrolls the mouse wheel over the window.
        /// </summary>
        public event Action<MouseScrollRelativeInput> MouseWheel;

        /// <summary>
        /// Invoked when the user moves the mouse cursor within the window.
        /// </summary>
        public event Action<MousePositionAbsoluteInput> MouseMove;

        /// <summary>
        /// Invoked when the user presses a mouse button.
        /// </summary>
        public event Action<MouseButtonInput> MouseDown;

        /// <summary>
        /// Invoked when the user releases a mouse button.
        /// </summary>
        public event Action<MouseButtonInput> MouseUp;

        /// <summary>
        /// Invoked when the user presses a key.
        /// </summary>
        public event Action<KeyboardKeyInput> KeyDown;

        /// <summary>
        /// Invoked when the user releases a key.
        /// </summary>
        public event Action<KeyboardKeyInput> KeyUp;

        /// <summary>
        /// Invoked when the user types a character.
        /// </summary>
        public event Action<char> KeyTyped;

        /// <summary>
        /// Invoked when the user drops a file into the window.
        /// </summary>
        public event Action<string> DragDrop;

        #endregion

        #region Event Invocation

        protected virtual void OnUpdate() => Update?.Invoke();
        protected virtual void OnResized() => Resized?.Invoke();
        protected virtual bool OnExitRequested() => ExitRequested?.Invoke() ?? false;
        protected virtual void OnExited() => Exited?.Invoke();
        protected virtual void OnFocusLost() => FocusLost?.Invoke();
        protected virtual void OnFocusGained() => FocusGained?.Invoke();
        protected virtual void OnShown() => Shown?.Invoke();
        protected virtual void OnHidden() => Hidden?.Invoke();
        protected virtual void OnMouseEntered() => MouseEntered?.Invoke();
        protected virtual void OnMouseLeft() => MouseLeft?.Invoke();
        protected virtual void OnMoved(Point point) => Moved?.Invoke(point);
        protected virtual void OnMouseWheel(MouseScrollRelativeInput evt) => MouseWheel?.Invoke(evt);
        protected virtual void OnMouseMove(MousePositionAbsoluteInput evt) => MouseMove?.Invoke(evt);
        protected virtual void OnMouseDown(MouseButtonInput evt) => MouseDown?.Invoke(evt);
        protected virtual void OnMouseUp(MouseButtonInput evt) => MouseUp?.Invoke(evt);
        protected virtual void OnKeyDown(KeyboardKeyInput evt) => KeyDown?.Invoke(evt);
        protected virtual void OnKeyUp(KeyboardKeyInput evt) => KeyUp?.Invoke(evt);
        protected virtual void OnKeyTyped(char c) => KeyTyped?.Invoke(c);
        protected virtual void OnDragDrop(string file) => DragDrop?.Invoke(file);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Window"/> using the specified window and graphics backends.
        /// </summary>
        /// <param name="windowBackend">The <see cref="IWindowBackend"/> to use.</param>
        /// <param name="graphicsBackend">The <see cref="IGraphicsBackend"/> to use.</param>
        public Window(IWindowBackend windowBackend, IGraphicsBackend graphicsBackend)
        {
            this.windowBackend = windowBackend;
            this.graphicsBackend = graphicsBackend;

            Position.ValueChanged += position_ValueChanged;
            Size.ValueChanged += size_ValueChanged;

            CursorState.ValueChanged += evt =>
            {
                this.windowBackend.CursorVisible = !evt.NewValue.HasFlag(Platform.CursorState.Hidden);
                this.windowBackend.CursorConfined = evt.NewValue.HasFlag(Platform.CursorState.Confined);
            };

            WindowState.ValueChanged += evt => this.windowBackend.WindowState = evt.NewValue;

            Visible.ValueChanged += visible_ValueChanged;

            focused.ValueChanged += evt =>
            {
                if (evt.NewValue)
                    OnFocusGained();
                else
                    OnFocusLost();
            };

            cursorInWindow.ValueChanged += evt =>
            {
                if (evt.NewValue)
                    OnMouseEntered();
                else
                    OnMouseLeft();
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the window's run loop.
        /// </summary>
        public void Run() => windowBackend.Run();

        /// <summary>
        /// Attempts to close the window.
        /// </summary>
        public void Close() => windowBackend.Close();

        /// <summary>
        /// Creates the concrete window implementation and initialises the graphics backend.
        /// </summary>
        public void Create()
        {
            windowBackend.Create();

            windowBackend.Resized += windowBackend_Resized;
            windowBackend.WindowStateChanged += () => WindowState.Value = windowBackend.WindowState;
            windowBackend.Moved += windowBackend_Moved;
            windowBackend.Hidden += () => Visible.Value = false;
            windowBackend.Shown += () => Visible.Value = true;

            windowBackend.FocusGained += () => focused.Value = true;
            windowBackend.FocusLost += () => focused.Value = false;
            windowBackend.MouseEntered += () => cursorInWindow.Value = true;
            windowBackend.MouseLeft += () => cursorInWindow.Value = false;

            windowBackend.Closed += OnExited;
            windowBackend.CloseRequested += OnExitRequested;
            windowBackend.Update += OnUpdate;
            windowBackend.KeyDown += OnKeyDown;
            windowBackend.KeyUp += OnKeyUp;
            windowBackend.KeyTyped += OnKeyTyped;
            windowBackend.MouseDown += OnMouseDown;
            windowBackend.MouseUp += OnMouseUp;
            windowBackend.MouseMove += OnMouseMove;
            windowBackend.MouseWheel += OnMouseWheel;
            windowBackend.DragDrop += OnDragDrop;

            windowBackend.DisplayChanged += d => CurrentDisplay.Value = d;

            graphicsBackend.Initialise(windowBackend);

            CurrentDisplay.Value = windowBackend.CurrentDisplay;
            CurrentDisplay.ValueChanged += evt => windowBackend.CurrentDisplay = evt.NewValue;
        }

        /// <summary>
        /// Requests that the graphics backend perform a buffer swap.
        /// </summary>
        public void SwapBuffers() => graphicsBackend.SwapBuffers();

        /// <summary>
        /// Requests that the graphics backend become the current context.
        /// May not be required for some backends.
        /// </summary>
        public void MakeCurrent() => graphicsBackend.MakeCurrent();

        public virtual void CycleMode()
        {
        }

        public virtual void SetupWindow(FrameworkConfigManager config)
        {
        }

        #endregion

        #region Bindable Handling

        private void visible_ValueChanged(ValueChangedEvent<bool> evt)
        {
            windowBackend.Visible = evt.NewValue;

            if (evt.NewValue)
                OnShown();
            else
                OnHidden();
        }

        private bool boundsChanging;

        private void windowBackend_Resized()
        {
            if (!boundsChanging)
            {
                boundsChanging = true;
                Position.Value = windowBackend.Position;
                Size.Value = windowBackend.Size;
                boundsChanging = false;
            }

            OnResized();
        }

        private void windowBackend_Moved(Point point)
        {
            if (!boundsChanging)
            {
                boundsChanging = true;
                Position.Value = point;
                boundsChanging = false;
            }

            OnMoved(point);
        }

        private void position_ValueChanged(ValueChangedEvent<Point> evt)
        {
            if (boundsChanging)
                return;

            boundsChanging = true;
            windowBackend.Position = evt.NewValue;
            boundsChanging = false;
        }

        private void size_ValueChanged(ValueChangedEvent<Size> evt)
        {
            if (boundsChanging)
                return;

            boundsChanging = true;
            windowBackend.Size = evt.NewValue;
            boundsChanging = false;
        }

        #endregion

        #region Deprecated IGameWindow

        public IWindowInfo WindowInfo => throw new NotImplementedException();

        osuTK.WindowState INativeWindow.WindowState
        {
            get => WindowState.Value.ToOsuTK();
            set => WindowState.Value = value.ToFramework();
        }

        public WindowBorder WindowBorder { get; set; }

        public Rectangle Bounds
        {
            get => new Rectangle(X, Y, Width, Height);
            set
            {
                Position.Value = value.Location;
                Size.Value = value.Size;
            }
        }

        public Point Location
        {
            get => Position.Value;
            set => Position.Value = value;
        }

        Size INativeWindow.Size
        {
            get => Size.Value;
            set => Size.Value = value;
        }

        public int X
        {
            get => Position.Value.X;
            set => Position.Value = new Point(value, Position.Value.Y);
        }

        public int Y
        {
            get => Position.Value.Y;
            set => Position.Value = new Point(Position.Value.X, value);
        }

        public int Width
        {
            get => Size.Value.Width;
            set => Size.Value = new Size(value, Size.Value.Height);
        }

        public int Height
        {
            get => Size.Value.Height;
            set => Size.Value = new Size(Size.Value.Width, value);
        }

        public Rectangle ClientRectangle
        {
            get => new Rectangle(Position.Value.X, Position.Value.Y, (int)(Size.Value.Width * Scale), (int)(Size.Value.Height * Scale));
            set
            {
                Position.Value = value.Location;
                Size.Value = new Size((int)(value.Width / Scale), (int)(value.Height / Scale));
            }
        }

        Size INativeWindow.ClientSize
        {
            get => new Size((int)(Size.Value.Width * Scale), (int)(Size.Value.Height * Scale));
            set => Size.Value = new Size((int)(value.Width / Scale), (int)(value.Height / Scale));
        }

        public MouseCursor Cursor { get; set; }

        public bool CursorVisible
        {
            get => windowBackend.CursorVisible;
            set => windowBackend.CursorVisible = value;
        }

        public bool CursorGrabbed
        {
            get => windowBackend.CursorConfined;
            set => windowBackend.CursorConfined = value;
        }

#pragma warning disable 0067

        public event EventHandler<EventArgs> Move;

        public event EventHandler<EventArgs> Resize;

        public event EventHandler<CancelEventArgs> Closing;

        event EventHandler<EventArgs> INativeWindow.Closed
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Disposed;

        public event EventHandler<EventArgs> IconChanged;

        public event EventHandler<EventArgs> TitleChanged;

        public event EventHandler<EventArgs> VisibleChanged;

        public event EventHandler<EventArgs> FocusedChanged;

        public event EventHandler<EventArgs> WindowBorderChanged;

        public event EventHandler<EventArgs> WindowStateChanged;

        event EventHandler<KeyboardKeyEventArgs> INativeWindow.KeyDown
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler<KeyPressEventArgs> KeyPress;

        event EventHandler<KeyboardKeyEventArgs> INativeWindow.KeyUp
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> MouseLeave;

        public event EventHandler<EventArgs> MouseEnter;

        event EventHandler<MouseButtonEventArgs> INativeWindow.MouseDown
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        event EventHandler<MouseButtonEventArgs> INativeWindow.MouseUp
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        event EventHandler<MouseMoveEventArgs> INativeWindow.MouseMove
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        event EventHandler<MouseWheelEventArgs> INativeWindow.MouseWheel
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler<FileDropEventArgs> FileDrop;

        public event EventHandler<EventArgs> Load;
        public event EventHandler<EventArgs> Unload;
        public event EventHandler<FrameEventArgs> UpdateFrame;
        public event EventHandler<FrameEventArgs> RenderFrame;

#pragma warning restore 0067

        bool IWindow.CursorInWindow => CursorInWindow.Value;

        CursorState IWindow.CursorState
        {
            get => CursorState.Value;
            set => CursorState.Value = value;
        }

        bool INativeWindow.Focused => Focused.Value;

        bool INativeWindow.Visible
        {
            get => Visible.Value;
            set => Visible.Value = value;
        }

        bool INativeWindow.Exists => Exists;

        public void Run(double updateRate) => Run();

        public void ProcessEvents()
        {
        }

        public Point PointToClient(Point point) => point;

        public Point PointToScreen(Point point) => point;

        public Icon Icon { get; set; }

        public void Dispose()
        {
        }

        #endregion
    }
}

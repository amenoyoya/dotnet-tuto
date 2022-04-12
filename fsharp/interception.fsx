open System
open System.Runtime.InteropServices

type InterceptionContext = IntPtr
type InterceptionDevice = int
type InterceptionFilter = ushort
type InterceptionPredicate = delegate of InterceptionDevice -> int

[<type:StructLayout(LayoutKind.Sequential)>]
type InterceptionMouseStroke =
    val mutable State: ushort
    val mutable Flags: ushort
    val mutable Rolling: short
    val mutable X: int
    val mutable Y: int
    val mutable Information: uint

[<type:StructLayout(LayoutKind.Sequential)>]
type InterceptionKeyStroke =
    val mutable Code: ushort
    val mutable State: ushort
    val mutable Information: uint

[<type:StructLayout(LayoutKind.Explicit)>]
type InterceptionStroke =
    [<FieldOffset(0)>] val mutable Mouse: InterceptionMouseStroke
    [<FieldOffset(0)>] val mutable Key: InterceptionKeyStroke

[<Flags>]
type KeyState =
    | KeyDown = 0x00
    | KeyUp = 0x01
    | KeyE0 = 0x02
    | KeyE1 = 0x04
    | KeySetLED = 0x08
    | KeyShadow = 0x10
    | KeyVKPacket = 0x20

[<Flags>]
type KeyStateFilter =
    | None = 0x0000
    | All = 0xFFFF
    | KeyDown = KeyState.KeyUp
    | KeyUp = KeyState.KeyUp <<< 1
    | KeyE0 = KeyState.KeyE0 <<< 1
    | KeyE1 = KeyState.KeyE1 <<< 1
    | KeySetLED = KeyState.KeySetLED <<< 1
    | KeyShadow = KeyState.KeyShadow <<< 1
    | KeyVKPacket = KeyState.KeyVKPacket <<< 1

[<Flags>]
type MouseState =
    | LeftButtonDown = 0x001
    | LeftButtonUp = 0x002
    | RightButtonDown = 0x004
    | RightButtonUp = 0x008
    | MiddleButtonDown = 0x010
    | MiddleButtonUp = 0x020
    | Button1Down = LeftButtonDown
    | Button1Up = LeftButtonUp
    | Button2Down = RightButtonDown
    | Button2Up = RightButtonUp
    | Button3Down = MiddleButtonDown
    | Button3Up = MiddleButtonUp
    | Button4Down = 0x040
    | Button4Up = 0x080
    | Button5Down = 0x100
    | Button5Up = 0x200
    | MouseWheel = 0x400
    | MouseHWheel = 0x800

[<Flags>]
type MouseStateFilter =
    | None = 0x0000
    | All = 0xFFFF
    | LeftButtonDown = MouseState.LeftButtonDown
    | LeftButtonUp = MouseState.LeftButtonUp
    | RightButtonDown = MouseState.RightButtonDown
    | RightButtonUp = MouseState.RightButtonUp
    | MiddleButtonDown = MouseState.MiddleButtonDown
    | MiddleButtonUp = MouseState.MiddleButtonUp
    | Button1Down = MouseState.Button1Down
    | Button1Up = MouseState.Button1Up
    | Button2Down = MouseState.Button2Down
    | Button2Up = MouseState.Button2Up
    | Button3Down = MouseState.Button3Down
    | Button3Up = MouseState.Button3Up
    | Button4Down = MouseState.Button4Down
    | Button4Up = MouseState.Button4Up
    | Button5Down = MouseState.Button5Down
    | Button5Up = MouseState.Button5Up
    | MouseWheel = MouseState.MouseWheel
    | MouseHWheel = MouseState.MouseHWheel
    | MouseMove = 0x1000

[<Flags>]
type MouseFlags =
    | MoveRelative = 0x000
    | MoveAbsolute = 0x001
    | VirtualDesktop = 0x002
    | AttributesChanged = 0x004
    | MoveNoCoalesce = 0x008
    | TermsvrSrcShadow = 0x100

/// interception.dll import module
module Interception =
    [<DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern InterceptionContext interception_create_context()

    [<DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void interception_destroy_context(InterceptionContext context)

    [<DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int64 interception_is_keyboard(InterceptionDevice device)

    [<DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern void interception_set_filter(InterceptionContext context, InterceptionPredicate predicate, InterceptionFilter filter)

    [<DllImport("interception.dll", CallingConvention = CallingConvention.Cdecl)>]
    extern int interception_receive(InterceptionContext context, InterceptionDevice device, InterceptionStroke& stroke, uint nstroke)

/// main process
let context = Interception.interception_create_context()

Interception.interception_set_filter(context, Interception.interception_is_keyboard, KeyStateFilter.KeyDown ||| KeyStateFilter.KeyUp)
if context <> IntPtr.Zero then
    Interception.interception_destroy_context(context)

namespace WebSharper.Notifications

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let NotificationPermission =
        Pattern.EnumStrings "NotificationPermission" [
            "default"
            "denied"
            "granted"
        ]

    let NotificationDir = 
        Pattern.EnumStrings "NotificationDir" [
            "auto"
            "ltr"
            "rtl"
        ]

    let NotificationAction =
        Pattern.Config "NotificationAction" {
            Required = [
                "action", T<string>
                "title", T<string>
            ]
            Optional = [
                "icon", T<string>
            ]
        }

    let NotificationOptions =
        Pattern.Config "NotificationOptions" {
            Required = []
            Optional = [
                "actions", !| NotificationAction.Type
                "badge", T<string>
                "body", T<string>
                "data", T<obj>
                "dir", NotificationDir.Type
                "icon", T<string>
                "image", T<string>
                "lang", T<string>
                "renotify", T<bool>
                "requireInteraction", T<bool>
                "silent", T<bool>
                "tag", T<string>
                "timestamp", T<int64>
                "vibrate", !| T<int>
            ]
        }

    let Notification =
        Class "Notification"
        |=> Inherits T<Dom.EventTarget>
        |+> Static [
            Constructor (T<string>?title * !?NotificationOptions?options)
            "requestPermission" => !?(NotificationPermission ^-> T<unit>)?callback ^-> T<Promise<string>>
            "permission" =? NotificationPermission
        ]
        |+> Instance [
            "title" =? T<string>
            "dir" =? NotificationDir.Type
            "lang" =? T<string>
            "body" =? T<string>
            "tag" =? T<string>
            "icon" =? T<string>
            "image" =? T<string>
            "badge" =? T<string>
            "timestamp" =? T<int64>
            "vibrate" =? !| T<int>
            "data" =? T<obj>
            "requireInteraction" =? T<bool>
            "renotify" =? T<bool>
            "silent" =? T<bool>
            "actions" =? !| NotificationAction.Type

            "close" => T<unit> ^-> T<unit>
        ]

    let NotificationEventOptions =
        Pattern.Config "NotificationEventOptions" {
            Required = [
                "notification", Notification.Type
            ]
            Optional = [
                "action", T<string>
            ]
        }

    let NotificationEvent =
        Class "NotificationEvent"
        |=> Inherits T<Dom.Event> 
        |+> Static [
            Constructor (T<string>?``type`` * !?NotificationEventOptions?options)
        ]       
        |+> Instance [
            "notification" =? Notification
            "action" =? T<string> 
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.Notifications" [
                NotificationPermission
                NotificationAction
                NotificationOptions
                NotificationDir
                Notification
                NotificationEventOptions
                NotificationEvent
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()

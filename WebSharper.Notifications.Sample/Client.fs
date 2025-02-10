namespace WebSharper.Notifications.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.UI.Notation
open WebSharper.Notifications

[<JavaScript>] 
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let status = Var.Create ""
    
    let requestNotification() = 
        promise {
            try
                let! permission = Notification.RequestPermission()
                if permission = "granted" then
                    status := "Notifications sent!"
                    let _ = new Notification("Hello! Notifications are enabled.", NotificationOptions(
                        Body = "This is a test notification."
                    ))
                    return ()
                else 
                    status := "Notifications denied!"
                    return ()
            with error ->
                status := $"{error.Message}"
                return ()
        }

    [<SPAEntryPoint>]
    let Main () =

        IndexTemplate.Main()
            .status(status.View)
            .requestNotification(fun _ -> 
                async {
                    do! requestNotification().AsAsync()
                } |> Async.Start
            )
            .Doc()
        |> Doc.RunById "main"

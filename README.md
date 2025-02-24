# WebSharper Notifications API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Notifications API](https://developer.mozilla.org/en-US/docs/Web/API/Notifications_API), enabling seamless integration of browser notifications into WebSharper applications.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Notifications API.

2. **Sample Project**:
   - Demonstrates how to use the Notifications API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/NotificationsAPI/).

## Features

- WebSharper bindings for the Notifications API.
- Simple API for displaying desktop notifications.
- Example usage for enhancing user engagement.
- Hosted demo to explore API functionality.

## Installation and Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/Notifications.git
   cd Notifications
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.Notifications/WebSharper.Notifications.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.Notifications.Sample
   dotnet build
   dotnet run
   ```

4. Open the hosted demo to see the Sample project in action:
   [https://dotnet-websharper.github.io/NotificationsAPI/](https://dotnet-websharper.github.io/NotificationsAPI/)

## Why Use the Notifications API

The Notifications API allows web applications to send notifications to users. Key benefits include:

1. **Improved User Engagement**: Notify users even when they are not actively using the application.
2. **Timely Alerts**: Inform users of important events in real time.
3. **Native Browser Integration**: Works with system-level notifications for a seamless experience.
4. **Customizable Messages**: Supports different types of messages, actions, and icons.

**Note:** If there is no prompted permission request for notifications, please allow notifications manually in your browser settings.

Integrating the Notifications API with WebSharper allows developers to create interactive and user-friendly web applications in F#.

## How to Use the Notifications API

### Example Usage

Below is an example of how to use the Notifications API in a WebSharper project:

```fsharp
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

    // Function to request notification permission and send a notification if granted
    let requestNotification() =
        promise {
            try
                // Request notification permission from the user
                let! permission = Notification.RequestPermission()
                if permission = "granted" then
                    status := "Notifications sent!"
                    // Create and display a new notification
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
        // Initialize the UI template and bind variables to UI elements
        IndexTemplate.Main()
            .status(status.View)
            .requestNotification(fun _ ->
                async {
                    do! requestNotification().AsAsync()
                } |> Async.Start
            )
            .Doc()
        |> Doc.RunById "main"
```

This example demonstrates how to request permission and display notifications dynamically.

For a complete implementation, refer to the [Sample Project](https://dotnet-websharper.github.io/NotificationsAPI/).

﻿using System;

using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace PSBattleHelper.Contracts.Services;

public interface IWebViewService
{
    bool CanGoBack
    {
        get;
    }

    bool CanGoForward
    {
        get;
    }

    event EventHandler<CoreWebView2WebErrorStatus> NavigationCompleted;

    void Initialize(WebView2 webView);

    void GoBack();

    void GoForward();

    void Reload();

    void UnregisterEvents();
}

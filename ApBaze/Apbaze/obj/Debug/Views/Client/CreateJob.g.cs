﻿#pragma checksum "..\..\..\..\Views\Client\CreateJob.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "23F990132D65B6563CE8ED43B25557353C05E66249AAAB9F5FC9AD7B95512F1B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using Notification.Wpf.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Apbaze {
    
    
    /// <summary>
    /// CreateJob
    /// </summary>
    public partial class CreateJob : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.DialogHost DialogHost;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label postedJobs;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label messages;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton themeToggle;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_exit;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Notification.Wpf.Controls.NotificationArea WindowArea;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox title;
        
        #line default
        #line hidden
        
        
        #line 156 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox description;
        
        #line default
        #line hidden
        
        
        #line 220 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox price;
        
        #line default
        #line hidden
        
        
        #line 232 "..\..\..\..\Views\Client\CreateJob.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox time;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Apbaze;component/views/client/createjob.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Client\CreateJob.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.DialogHost = ((MaterialDesignThemes.Wpf.DialogHost)(target));
            return;
            case 2:
            
            #line 45 "..\..\..\..\Views\Client\CreateJob.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.FindWorkRedirect);
            
            #line default
            #line hidden
            return;
            case 3:
            this.postedJobs = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            
            #line 49 "..\..\..\..\Views\Client\CreateJob.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.MessagesRedirect);
            
            #line default
            #line hidden
            return;
            case 5:
            this.messages = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            
            #line 55 "..\..\..\..\Views\Client\CreateJob.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Label_MouseDown);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 59 "..\..\..\..\Views\Client\CreateJob.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.PackIcon_MouseDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.themeToggle = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 67 "..\..\..\..\Views\Client\CreateJob.xaml"
            this.themeToggle.Click += new System.Windows.RoutedEventHandler(this.toggleTheme);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btn_exit = ((System.Windows.Controls.Button)(target));
            
            #line 69 "..\..\..\..\Views\Client\CreateJob.xaml"
            this.btn_exit.Click += new System.Windows.RoutedEventHandler(this.exitApp);
            
            #line default
            #line hidden
            return;
            case 10:
            this.WindowArea = ((Notification.Wpf.Controls.NotificationArea)(target));
            return;
            case 11:
            
            #line 101 "..\..\..\..\Views\Client\CreateJob.xaml"
            ((System.Windows.Controls.StackPanel)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.BackRedirect);
            
            #line default
            #line hidden
            return;
            case 12:
            this.title = ((System.Windows.Controls.TextBox)(target));
            return;
            case 13:
            this.description = ((System.Windows.Controls.TextBox)(target));
            return;
            case 14:
            
            #line 171 "..\..\..\..\Views\Client\CreateJob.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 172 "..\..\..\..\Views\Client\CreateJob.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 16:
            this.price = ((System.Windows.Controls.TextBox)(target));
            return;
            case 17:
            this.time = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


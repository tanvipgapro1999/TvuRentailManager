   M S B A M L   `   `   `  ????G  CTRMDesktopUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:TRMDesktopUI.ViewsTRMDesktopUI.Views  5      "    TRMDesktopUI.Views.LoginView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:TRMDesktopUI.Views     5      
???????$??24(?5   2   .??5	      ?   ? ColumnDefinitions  5
   
   ?? 5      $??* ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $??* ?6    ? RowDefinitions 5   
   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $??* ?6   .+?5   
   ?? $??0??6   $??1??6!   $??2??61   $??0 0 0 20q?6E   $??Center=?5      $??64(?61   $??Segoe UI Light)?6?   
Login Form5      ?? 5   
   -   5         ??Name$ ErrorMessage??5      $??1??5      $??1??6!   $??2??61   $??0 0 0 20q?5      
???  ??$	??300??67   $
??Wrap=?5       y?5   *   ?? *IsErrorVisible+    BooleanToVisibilityConverter ?? 	Converter# [   ?? FallbackValue$ 	Collapsed??5       ?? 5#   
   $??2??6   $??1??6!   $??0 0 5 10q?61   	Username:5%      ?? 5&   
   -   5         ??Name$ userName??5&      $??2??6$   $??2??61   $	??350??6A   $??0 0 0 10q?5'      ?? 5)   
   $??3??6   $??1??6!   $??0 0 5 10q?61   	Password:5+      G? 5,   
   -   5         G?Name$ Password??5,      $??3??6(   $??2??65   $	??350??6E   $??0 0 0 20q?5-      ?? 5/   
   -   5         ??Name$ LogIn??50   .   $??4??5/      $??1??6   $??2??6.   $
??20 5q?6B   $??Center=?50      .??6=   Log In52      53      54      
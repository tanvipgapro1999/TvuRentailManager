   M S B A M L   `   `   `  ????G  CTRMDesktopUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:TRMDesktopUI.ViewsTRMDesktopUI.Views  5      (    "TRMDesktopUI.Views.UserDisplayView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:TRMDesktopUI.Views     5      
???????$??24(?5   ?   .??5	      ?   ? RowDefinitions  5
   
   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $??* ?6    ? ColumnDefinitions 5   
   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $
??auto ?6   ?? 5      $??* ?6   .+?5   
   ?? $??User Administration??6   $??48(?6/   $??0??6=   $??0??5      $??3??6$   $??0 0 0 20q?68   ?? 5!   
   $??Users??6   $??0??6!   $??1??61   ?? 5"   
   -   5         ??Name$ Users??5"      $??1??6!   $??0??6.   $	??200??5#      $	??150??6#    ?? SelectedItem$ SelectedUser??62   $??0,34,0,0q?6N   $??2??6`   ??5$      ?? 5%      .R?5&      ??  ?? Orientation$ Vertical=?6!   .+?5'      ?? ??6$   ?? *Email+?? 5(      $??18(?6>   ??6$   ?? *
RoleList+6>   5)      5*      5,      ?? 5.   
   $ Vertical=?6   $??1??6,   $??1??69   $??3??6I   $??20 0 0 0q?6]   .+?5/      ?? -   5         ??Name$ SelectedUserName??5/      $??0 0 0 10q?62   ?? 50      $??Current Roles??6   ?? 51      $ 
Horizontal=?6   $??0 0 0 10q?62   .+?52      ?? -   5        $ 	UserRoles??52      60   ?? 53      -   5         ??Name$ RemoveSelectedRole??53      $??Remove??65   $??5 0 0 0q?6F   54      ?? 56      $ 
Horizontal=?6   .+?57      ?? -   5         ??Name$ AvailableRoles??57      ?? 58      -   5        $ AddSelectedRole??58      $??Add Role??62   $??5 0 0 0q?6E   59      5:      5;      5<      
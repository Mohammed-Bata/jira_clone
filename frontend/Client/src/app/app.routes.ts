import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Authcallback } from './features/auth/authcallback/authcallback';
import { Navbar } from './shared/components/navbar/navbar';
import { Sidebar } from './shared/components/sidebar/sidebar';

export const routes: Routes = [
    {
        path:'login',
        component:Login
    },
    {
        path: 'oauth-callback',
        component:Authcallback
    },
    {
        path:'nav',
        component: Navbar
    },
    {
        path:'side',
        component: Sidebar
    }
];

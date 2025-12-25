import { Routes } from '@angular/router';
import { Login }  from './pages/Login/login';
import { Registration } from './pages/registration/registration';
import { Home } from './pages/home/home';

export const routes: Routes = [
    {
        path:'',
        component:Home
    },
    {
        path:'Registration',
        component:Registration
    },
    {
        path: 'Login',
        component: Login,
    }
];

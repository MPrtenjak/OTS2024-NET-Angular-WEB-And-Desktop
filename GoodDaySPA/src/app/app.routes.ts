import { Routes } from '@angular/router';
import { LoginComponent } from '@pages/login/login.component';
import { NotesComponent } from '@pages/notes/notes.component';
import { SignInComponent } from '@pages/sign-in/sign-in.component';
import { InfoComponent } from '@pages/info/info.component';
import { SlideshowComponent } from '@pages/slideshow/slideshow.component';
import { AuthGuard } from '@app/auth.guard';

export const routes: Routes = [
    { path: 'sign-in', component: SignInComponent },
    { path: 'login', component: LoginComponent },
    { path: 'slideshow', component: SlideshowComponent, canActivate: [AuthGuard] },
    { path: 'slideshow/:slide-id', component: SlideshowComponent, canActivate: [AuthGuard] },
    { path: 'notes', component: NotesComponent, canActivate: [AuthGuard] },
    { path: 'info', component: InfoComponent, canActivate: [AuthGuard] },
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: '**', redirectTo: '/login' }
];

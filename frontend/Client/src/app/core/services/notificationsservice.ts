import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.development";
import { BehaviorSubject } from "rxjs";
import { NotificationDto, UnreadCount } from "../models/Notification";
import { API_ENDPOINTS } from "../constants/api-endpoints";
import * as signalR from '@microsoft/signalr';
import { HttpClient } from "@angular/common/http";


@Injectable({ 
    providedIn: 'root'
})

export class NotificationsService{
    private readonly apiUrl = environment.apiUrl;
    private hubConnection!: signalR.HubConnection;

    private notificationsSubject = new BehaviorSubject<NotificationDto[]>([])
    public notifications$ = this.notificationsSubject.asObservable();

    private unreadCountSubject = new BehaviorSubject<number>(0);
    public unreadCount$ = this.unreadCountSubject.asObservable();

    constructor(private http:HttpClient){}

    getUnreadCount():void{
        this.http.get<UnreadCount>(`${this.apiUrl}${API_ENDPOINTS.Notification.UNREADCOUNT}`).subscribe({
            next:(response)=>this.unreadCountSubject.next(response.unread)
        })
    }

    loadNotifications(): void{
        console.log('inthemethod');
        if(this.notificationsSubject.value.length > 0){
            console.log(this.notificationsSubject.value.length);
            console.log(this.notificationsSubject.value);
            return;
        }
        console.log('getting');
        this.http.get<NotificationDto[]>(`${this.apiUrl}${API_ENDPOINTS.Notification.GETALL}`,{params:{
            Status:'all'
        },withCredentials:true}).subscribe({
            next:(notifications)=>{
                console.log(notifications);
                this.notificationsSubject.next(notifications);
            }
        })
    }

    startConnection(token:string){
        this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl('https://localhost:7144/notifications',{
            accessTokenFactory:()=>token 
        })
        .withAutomaticReconnect()
        .build();

        this.hubConnection.start()
        .then(() => console.log('SignalR: Connected with JWT via Query String'))
        .catch(err => console.error('SignalR Connection Error: ', err));

        this.hubConnection.on('ReceiveNotification', (notification: any) => {
            const current = this.notificationsSubject.value;
            this.notificationsSubject.next([notification, ...current]);

            const count = this.unreadCountSubject.value;
            this.unreadCountSubject.next(count + 1);
        });
    }
}
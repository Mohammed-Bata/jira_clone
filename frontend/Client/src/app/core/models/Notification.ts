export interface NotificationDto{
    id:number,
    message:string,
    actorname:string,
    createdat:string,
    isread:boolean
}

export interface UnreadCount{
    unread:number
}
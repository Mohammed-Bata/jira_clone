import { DELETE, R } from "@angular/cdk/keycodes";

export const API_ENDPOINTS = {
  AUTH: {
    ME: '/Users/me',
    LOGIN: '/Users/login',
    REGISTER: '/Users/register',
    REFRESH: '/Users/refresh',
    LOGOUT: '/Users/logout',
    GOOGLELOGIN: '/Users/google/login'
  },
  PROJECT: {
    CREATE: '/Projects/create',
    GETALL:'/Projects/all',
    GETPROJECT:'/Projects'
  },
  WORKITEMS : {
    CREATE: '/WorkItems/create',
    DELETE: '/WorkItems',
    REORDER: '/WorkItems/reorder'
  },
  Column:{
    CREATE: '/Columns/create',
    REORDER: '/Columns/reorder',
    DELETE: '/Columns'
  },
  TeamMembers:{
    INVITE: '/TeamMembers/invite',
    ACCEPT: '/TeamMembers/accept'
  },
  Notification:{
    UNREADCOUNT:'/Notifications/unread',
    MARKALLREAD:'/Notifications/markallread',
    GETALL:'/Notifications/all'
  }
};
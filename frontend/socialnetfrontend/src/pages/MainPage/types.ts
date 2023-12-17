
export const drawerWidth = 240;
export type PageType = 'Profile' | 'Friends' | 'Feed' | 'Dialogs' | 'Search'

export const pages = new Map<string,PageType>([
    ['Профиль','Profile' ],
    ['Друзья','Friends'],
    ['Лента','Feed'],
    ['Диалоги','Dialogs'],
    ['Поиск','Search'],
])
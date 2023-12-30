import { PageType } from "../../core/types";

export const drawerWidth = 240;

export const pages = new Map<string,PageType>([
    ['Профиль','Profile' ],
    ['Подписки','Friends'],
    ['Лента','Feed'],
    ['Собеседники','Dialogs'],
    ['Поиск','Search'],
])
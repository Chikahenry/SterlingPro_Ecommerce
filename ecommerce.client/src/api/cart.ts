import apiClient from './client';

export interface CartItem {
    id: string;
    productId: string;
    productName: string;
    productPrice: number;
    quantity: number;
}

export interface Cart {
    id: string;
    userId: string;
    total: number;
    items: CartItem[];
}

export const getCart = async (userId: string): Promise<Cart> => {
    const response = await apiClient.get<Cart>(`/carts?userId=${userId}`);
    return response.data;
};

export const addItemToCart = async (userId: string, productId: string, quantity: number): Promise<Cart> => {
    const response = await apiClient.post<Cart>('/carts/items', { userId, productId, quantity });
    return response.data;
};

export const updateCartItem = async (itemId: string, userId: string, quantity: number): Promise<Cart> => {
    const response = await apiClient.put<Cart>(`/carts/items/${itemId}`, { itemId, userId, quantity });
    return response.data;
};

export const removeItemFromCart = async (itemId: string, userId: string): Promise<Cart> => {
    const response = await apiClient.delete<Cart>(`/carts/items/${itemId}?userId=${userId}`);
    return response.data;
};
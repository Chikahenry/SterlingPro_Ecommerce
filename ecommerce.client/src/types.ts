// Product type matching the backend DTO
export interface Product {
    id: string;
    name: string;
    description: string;
    price: number;
    stockQuantity: number;
    imageUrl: string;
}

// CartItem type
export interface CartItem {
    id: string;
    productId: string;
    productName: string;
    productPrice: number;
    quantity: number;
}

// Cart type
export interface Cart {
    id: string;
    userId: string;
    total: number;
    items: CartItem[];
}

// User type for authentication
export interface User {
    id: string;
    username: string;
}

// Auth response type
export interface AuthResponse {
    userId: string;
    username: string;
    token: string;
}

// Error response type
export interface ApiError {
    message: string;
    statusCode?: number;
}

// Generic API response type
export interface ApiResponse<T> {
    data?: T;
    error?: ApiError;
}
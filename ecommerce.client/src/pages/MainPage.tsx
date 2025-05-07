import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Grid, Card, CardContent, CardMedia, Typography, IconButton, Badge, AppBar, Toolbar } from '@mui/material';
import { ShoppingCart } from '@mui/icons-material';
import { getProducts } from '../api/products';
import { getCart, addItemToCart } from '../api/cart';
import { Product, Cart } from '../types';

const MainPage: React.FC = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [cart, setCart] = useState<Cart | null>(null);
    const [showCart, setShowCart] = useState(false);
    const userId = localStorage.getItem('userId');
    const navigate = useNavigate();

    useEffect(() => {
        if (!userId) {
            navigate('/login');
            return;
        }

        const fetchData = async () => {
            try {
                const [productsData, cartData] = await Promise.all([
                    getProducts(),
                    getCart(userId),
                ]);
                setProducts(productsData);
                setCart(cartData);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchData();
    }, [userId, navigate]);

    const handleAddToCart = async (productId: string) => {
        if (!userId) return;
        try {
            const updatedCart = await addItemToCart(userId, productId, 1);
            setCart(updatedCart);
        } catch (error) {
            console.error('Error adding to cart:', error);
        }
    };

    const cartItemCount = cart?.items.reduce((total, item) => total + item.quantity, 0) || 0;

    return (
        <div>
            <AppBar position="static">
                <Toolbar>
                    <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                        E-Commerce Store
                    </Typography>
                    <IconButton color="inherit" onClick={() => setShowCart(!showCart)}>
                        <Badge badgeContent={cartItemCount} color="error">
                            <ShoppingCart />
                        </Badge>
                    </IconButton>
                </Toolbar>
            </AppBar>

            <Grid container spacing={2} sx={{ p: 2 }}>
                {products.map((product) => (
                    <Grid item xs={12} sm={6} md={4} key={product.id}>
                        <Card>
                            <CardMedia
                                component="img"
                                height="140"
                                image={product.imageUrl || 'https://via.placeholder.com/150'}
                                alt={product.name}
                            />
                            <CardContent>
                                <Typography gutterBottom variant="h5" component="div">
                                    {product.name}
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    {product.description}
                                </Typography>
                                <Typography variant="h6" sx={{ mt: 1 }}>
                                    ${product.price.toFixed(2)}
                                </Typography>
                                <Button
                                    variant="contained"
                                    sx={{ mt: 1 }}
                                    onClick={() => handleAddToCart(product.id)}
                                >
                                    Add to Cart
                                </Button>
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>

            {/* Cart Drawer would go here */}
        </div>
    );
};

export default MainPage;
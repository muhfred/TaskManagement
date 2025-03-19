import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// This function can be marked `async` if using `await` inside
export function middleware(request: NextRequest) {
  // Get the pathname of the request
  const path = request.nextUrl.pathname;

  // Define public paths that don't require authentication
  const isPublicPath = path === '/login' || path === '/register';

  // Check if the user is authenticated (has a token)
  const token = request.cookies.get('auth_token')?.value || '';
  const isAuthenticated = !!token;

  // If the path requires authentication and the user is not authenticated,
  // redirect to the login page
  if (!isPublicPath && !isAuthenticated) {
    return NextResponse.redirect(new URL('/login', request.url));
  }

  // If the user is authenticated and trying to access login/register,
  // redirect to the home page
  if (isPublicPath && isAuthenticated) {
    return NextResponse.redirect(new URL('/', request.url));
  }

  // Otherwise, continue with the request
  return NextResponse.next();
}

// Specify the paths that should be checked by the middleware
export const config = {
  matcher: ['/', '/login', '/register'],
};

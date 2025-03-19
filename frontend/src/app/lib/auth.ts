'use client';

import { useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import { isAuthenticated } from './api';

export function useAuth() {
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    // Check if the route requires authentication
    const isPublicPath = pathname === '/login' || pathname === '/register';

    if (!isPublicPath && !isAuthenticated()) {
      router.push('/login');
    } else if (isPublicPath && isAuthenticated()) {
      router.push('/');
    }
  }, [pathname, router]);

  return { isAuthenticated: isAuthenticated() };
}

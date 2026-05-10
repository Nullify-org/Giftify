/* ============================================================
   site.js  –  Giftify global scripts
   ============================================================ */

// ── Cart badge ───────────────────────────────────────────────
function updateCartBadge(count) {
    // support both id="cart-badge" and class="cart-badge"
    const badges = [
        document.getElementById('cart-badge'),
        ...document.querySelectorAll('.cart-badge')
    ].filter(Boolean);

    badges.forEach(badge => {
        badge.textContent = count;
        badge.style.display = count > 0 ? 'inline-flex' : 'none';
    });
}

// Fetch cart count on every page load
document.addEventListener('DOMContentLoaded', function () {
    fetch('/Cart/Count')
        .then(r => r.json())
        .then(data => updateCartBadge(data.cartCount))
        .catch(() => {});
});

// ── Toast notifications ──────────────────────────────────────
function showToast(message, type = 'success') {
    let container = document.getElementById('gfy-toast-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'gfy-toast-container';
        Object.assign(container.style, {
            position: 'fixed', bottom: '24px', right: '24px',
            zIndex: '9999', display: 'flex', flexDirection: 'column', gap: '10px'
        });
        document.body.appendChild(container);
    }

    const bg   = type === 'success' ? '#0F3D2E' : '#dc3545';
    const icon = type === 'success' ? '✓' : '✕';

    const toast = document.createElement('div');
    Object.assign(toast.style, {
        background: bg, color: '#fff',
        padding: '14px 20px', borderRadius: '12px',
        fontSize: '0.9rem', display: 'flex', alignItems: 'center',
        gap: '10px', minWidth: '260px',
        boxShadow: '0 8px 24px rgba(0,0,0,.22)',
        opacity: '0', transform: 'translateY(12px)',
        transition: 'opacity .25s ease, transform .25s ease'
    });
    toast.innerHTML = `<span style="font-size:1rem">${icon}</span><span>${message}</span>`;
    container.appendChild(toast);

    requestAnimationFrame(() => requestAnimationFrame(() => {
        toast.style.opacity = '1';
        toast.style.transform = 'translateY(0)';
    }));
    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateY(12px)';
        setTimeout(() => toast.remove(), 300);
    }, 3200);
}

// ── Get CSRF token ───────────────────────────────────────────
function getCsrfToken() {
    const meta = document.querySelector('meta[name="csrf-token"]');
    if (meta) return meta.content;
    const input = document.querySelector('input[name="__RequestVerificationToken"]');
    return input ? input.value : '';
}

// ── Add to cart ──────────────────────────────────────────────
function addToCart(productId, quantity) {
    quantity = parseInt(quantity) || 1;
    const token = getCsrfToken();

    if (!token) {
        showToast('Please sign in to add items to your cart.', 'error');
        setTimeout(() => window.location.href = '/Account/Login', 1600);
        return Promise.reject('no-token');
    }

    return fetch('/Cart/Add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ productId: productId, quantity: quantity })
    })
    .then(async r => {
        const data = await r.json().catch(() => ({}));

        if (r.status === 401) {
            showToast('Please sign in to add items to your cart.', 'error');
            setTimeout(() => window.location.href = '/Account/Login', 1600);
            return;
        }
        if (!r.ok || !data.success) {
            showToast(data.message || 'Something went wrong. Please try again.', 'error');
            return;
        }
        updateCartBadge(data.cartCount);
        showToast(data.message || 'Added to cart! 🎁', 'success');
    })
    .catch(() => showToast('Network error. Please try again.', 'error'));
}

// ── Delegate click handler for [data-product-id] buttons ─────
document.addEventListener('DOMContentLoaded', function () {
    document.body.addEventListener('click', function (e) {
        const btn = e.target.closest('[data-product-id]');
        if (!btn || btn.disabled) return;
        if (btn.dataset.useQty === 'true') return;

        const productId = parseInt(btn.dataset.productId);
        const quantity  = parseInt(btn.dataset.quantity || '1');
        if (!productId) return;

        const originalHtml = btn.innerHTML;
        btn.disabled = true;
        btn.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span> Adding…';

        addToCart(productId, quantity).finally(() => {
            setTimeout(() => {
                btn.disabled = false;
                btn.innerHTML = originalHtml;
            }, 900);
        });
    });
});

// ── Mobile menu ──────────────────────────────────────────────
function toggleMobileMenu() {
    const menu = document.getElementById('mobileMenu');
    if (menu) menu.classList.toggle('show');
}
window.addEventListener('click', function (e) {
    const menu = document.getElementById('mobileMenu');
    const btn  = document.querySelector('.menu-toggle-btn');
    if (menu && btn && !btn.contains(e.target) && !menu.contains(e.target)) {
        menu.classList.remove('show');
    }
});


(function () {
    'use strict';

    function getOrCreateBadge() {
        let badge = document.querySelector('.cart-nav-badge');
        if (!badge) {
            const cartLink = document.querySelector('a[href*="/Cart"]');
            if (!cartLink) return null;
            cartLink.style.position = 'relative';
            badge = document.createElement('span');
            badge.className = 'badge rounded-pill bg-dark position-absolute top-0 start-100 translate-middle cart-nav-badge';
            badge.style.display = 'none';
            cartLink.appendChild(badge);
        }
        return badge;
    }

    function updateBadge(count) {
        const badge = getOrCreateBadge();
        if (!badge) return;
        if (count > 0) {
            badge.textContent = count;
            badge.style.display = '';
        } else {
            badge.style.display = 'none';
        }
    }

    function showToast(message, isSuccess) {
        document.querySelectorAll('.cart-toast').forEach(t => t.remove());

        const toast = document.createElement('div');
        toast.className = 'cart-toast';
        toast.setAttribute('role', 'alert');
        toast.style.cssText = [
            'position:fixed',
            'bottom:24px',
            'right:24px',
            'z-index:9999',
            'background:' + (isSuccess ? '#0F3D2E' : '#ef4444'),
            'color:#fff',
            'padding:.75rem 1.25rem',
            'border-radius:12px',
            'font-size:.9rem',
            'font-weight:600',
            'box-shadow:0 4px 18px rgba(0,0,0,.18)',
            'opacity:0',
            'transform:translateY(12px)',
            'transition:opacity .25s,transform .25s',
        ].join(';');
        toast.textContent = message;
        document.body.appendChild(toast);

        // Animate in
        requestAnimationFrame(() => {
            toast.style.opacity = '1';
            toast.style.transform = 'translateY(0)';
        });

        setTimeout(() => {
            toast.style.opacity = '0';
            toast.style.transform = 'translateY(12px)';
            toast.addEventListener('transitionend', () => toast.remove(), { once: true });
        }, 2500);
    }

    function setButtonLoading(btn, loading) {
        if (loading) {
            btn.dataset.origHtml = btn.innerHTML;
            btn.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';
            btn.disabled = true;
        } else {
            btn.innerHTML = btn.dataset.origHtml || btn.innerHTML;
            btn.disabled = false;
        }
    }

    document.addEventListener('submit', async function (e) {
        const form = e.target;
        if (form.dataset.cartAjax !== 'true') return;  

        e.preventDefault();
        e.stopPropagation();

        const btn = form.querySelector('button[type="submit"]');
        if (btn) setButtonLoading(btn, true);

        try {
            const res = await fetch(form.action, {
                method: 'POST',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: new URLSearchParams(new FormData(form)).toString(),
            });

            const data = await res.json();
            showToast(data.message, data.success);
            if (data.success) updateBadge(data.cartCount);
        } catch {
            showToast('Something went wrong. Please try again.', false);
        } finally {
            if (btn) setButtonLoading(btn, false);
        }
    });
})();

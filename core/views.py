from django.http import JsonResponse
from django.shortcuts import get_object_or_404
from django.views.decorators.http import require_GET

from .models import Wallet


@require_GET
def health(request):
    data = {
        'status': 'ok'
    }
    return JsonResponse(data)


@require_GET
def wallet_detail(request, wallet_id):
    wallet = get_object_or_404(Wallet, pk=wallet_id)
    data = {
        'id': wallet.id,
        'owner': wallet.owner.username,
        'balance': str(wallet.balance),
        'currency': 'LAK',
    }
    return JsonResponse(data)

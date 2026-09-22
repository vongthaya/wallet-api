from django.shortcuts import render
from django.http import JsonResponse


def health(request):
    data = {
        'status': 'ok'
    }
    return JsonResponse(data)
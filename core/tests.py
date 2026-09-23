from django.contrib.auth import get_user_model
from django.test import SimpleTestCase, TestCase
from django.urls import reverse

from .models import Wallet


class HealthTests(SimpleTestCase):
    def test_health(self):
        response = self.client.get('/health/')
        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.json(), {'status': 'ok'})


class WalletDetailTest(TestCase):
    def setUp(self):
        # setup user
        User = get_user_model()
        self.user = User.objects.create_user(username='kou', password='1234')

        # setup wallet
        self.wallet = Wallet.objects.create(owner=self.user, balance=100000)

    def test_wallet_detail_returns_200(self):
        response = self.client.get(reverse('wallet-detail', args=[self.wallet.pk]))
        self.assertEqual(response.status_code, 200)
        expected_data = {
            'id': self.wallet.pk,
            'owner': self.user.username,
            'balance': '100000',
            'currency': 'LAK',
        }
        self.assertEqual(response.json(), expected_data)

    def test_wallet_detail_returns_404_when_missing(self):
        response = self.client.get(reverse('wallet-detail', args=[self.wallet.pk + 1]))
        self.assertEqual(response.status_code, 404)

    def test_wallet_detail_rejects_post(self):
        response = self.client.post(reverse('wallet-detail', args=[self.wallet.pk]))
        self.assertEqual(response.status_code, 405)

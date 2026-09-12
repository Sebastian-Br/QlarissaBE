import asyncio
import yfinance as yf


async def main():
    async with yf.AsyncWebSocket() as ws:
        await ws.subscribe("TSLA")

        print("Subscribed to 005930.KS. Waiting for data...")

        await ws.listen(
            message_handler=lambda message: print(message)
        )


asyncio.run(main())
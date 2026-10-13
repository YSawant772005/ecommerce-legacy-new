import type { Product } from '../types'

type ImageProduct = Pick<Product, 'id' | 'name' | 'categoryName'>

const image = (id: string) => `https://images.unsplash.com/${id}?auto=format&fit=crop&w=900&q=85`

const categoryImages: Record<string, string[]> = {
  electronics: ['photo-1496181133206-80ce9b88a853', 'photo-1511707171634-5f897ff02aa9', 'photo-1526738549149-8e07eca6c147', 'photo-1550009158-9ebf69173e03', 'photo-1593642632823-8f785ba67e45', 'photo-1541807084-5c52b6b3adef'],
  fashion: ['photo-1521572163474-6864f9cf17ab', 'photo-1483985988355-763728e1935b', 'photo-1490481651871-ab68de25d43d', 'photo-1525507119028-ed4c629a60a3', 'photo-1512436991641-6745cdb1723f', 'photo-1445205170230-053b83016050'],
  'home-kitchen': ['photo-1556228453-efd6c1ff04f6', 'photo-1618220179428-22790b461013', 'photo-1556911220-bff31c812dba', 'photo-1586023492125-27b2c045efd7', 'photo-1494438639946-1ebd1d20bf85', 'photo-1600566753086-00f18fb6b3ea'],
  books: ['photo-1544947950-fa07a98d237f', 'photo-1495446815901-a7297e633e8d', 'photo-1512820790803-83ca734da794', 'photo-1526243741027-444d633d7365', 'photo-1519682337058-a94d519337bc', 'photo-1532012197267-da84d127e765'],
  sports: ['photo-1461896836934-ffe607ba8211', 'photo-1517836357463-d25dfeac3438', 'photo-1534438327276-14e5300c3a48', 'photo-1552674605-db6ffd4facb5', 'photo-1579952363873-27f3bade9f55', 'photo-1546519638-68e109498ffc'],
  beauty: ['photo-1596462502278-27bfdc403348', 'photo-1522335789203-aabd1fc54bc9', 'photo-1515377905703-c4788e51af15', 'photo-1598440947619-2c35fc9aa908', 'photo-1556229010-6c3f2c9ca5f8', 'photo-1571781926291-c477ebfd024b'],
  toys: ['photo-1596461404969-9ae70f2830c1', 'photo-1560961911-ba7ef651a56c', 'photo-1599623560574-39d485900c95', 'photo-1594736797933-d0501ba2fe65', 'photo-1587654780291-39c9404d746b', 'photo-1566576912321-d58ddd7a6088'],
  automotive: ['photo-1503736334956-4c8f8e92946d', 'photo-1492144534655-ae79c964c9d7', 'photo-1549317661-bd32c8ce0db2', 'photo-1553440569-bcc63803a83d', 'photo-1503376780353-7e6692767b70', 'photo-1493238792000-8113da705763'],
  garden: ['photo-1416879595882-3373a0480b5b', 'photo-1585320806297-9794b3e4eeae', 'photo-1599685315640-9a85b5f5e3e1', 'photo-1558618666-fcd25c85cd64', 'photo-1591857177580-dc82b9ac4e1e', 'photo-1464226184884-fa280b87c399'],
  health: ['photo-1505751172876-fa1923c5c528', 'photo-1542736667-069246bdbc74', 'photo-1517832207067-4db24a2ae47c', 'photo-1559757175-0eb30cd8c063', 'photo-1576091160399-112ba8d25d1d', 'photo-1532938911079-1b06ac7ceec7'],
  stationery: ['photo-1517842645767-c639042777db', 'photo-1455390582262-044c70d4a48c', 'photo-1516979187457-637abb4f9353', 'photo-1544816155-12df9643f363', 'photo-1499951360447-b19be8fe80f5', 'photo-1586953208448-b95a79798f07'],
  'pet-supplies': ['photo-1587300003388-59208cc962cb', 'photo-1558788353-f76d92427f16', 'photo-1548199973-03cce0bbc87b', 'photo-1517849845537-4d257902454a', 'photo-1583511655857-d19b40a7a54e', 'photo-1537151608828-ea2b11777ee8']
}

export function productImage(product: ImageProduct): string {
  const category = product.categoryName?.toLowerCase().replace(/\s+/g, '-') || 'electronics'
  const images = categoryImages[category] || categoryImages.electronics
  const key = `${product.id}-${product.name}`
  const hash = Array.from(key).reduce((total, character) => total + character.charCodeAt(0), 0)
  return image(images[hash % images.length])
}

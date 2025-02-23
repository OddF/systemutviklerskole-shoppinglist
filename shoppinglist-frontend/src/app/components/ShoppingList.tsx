'use client'

import React, { useState, useEffect } from 'react';

interface ShoppingItem {
  rowKey: string;
  name: string;
  category: string;
}

let dev = false;
if (typeof window !== "undefined") {
  dev = window.location.href === "http://localhost:3000/";
}

const url = dev ? 'http://localhost:5199/api/shoppingitems' : '/api/shoppingitems'

const ShoppingList: React.FC = () => {
  const [items, setItems] = useState<ShoppingItem[]>([]);
  const [newItem, setNewItem] = useState<string>('');

  useEffect(() => {
    console.log("fetcing", url);

    fetch(url)
      .then(response => response.json())
      .then(data => setItems(data));
  }, []);

  const addItem = () => {
    console.log("Adding item", url);
    fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ name: newItem, category: 'Uncategorized' }),
    })
      .then(response => response.json())
      .then(item => setItems([...items, item]));
    setNewItem('');
  };

  return (
    <div className="container mx-auto p-4">
      <div className="mb-4">
        <input
          type="text"
          className="border p-2 mr-2"
          placeholder="New Item"
          value={newItem}
          onChange={(e) => setNewItem(e.target.value)}
        />
        <button
          onClick={addItem}
          className="bg-blue-500 text-white p-2 rounded"
        >
          Add Item
        </button>
      </div>
      <ul>
        {items.map((item) => (
          <li key={item.rowKey} className="border-b p-2">
            {item.name} - {item.category}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ShoppingList;
import { PrismaClient } from "./prisma/client/client.ts";
const prisma = new PrismaClient();

const seed_users = [
    {name: "Ben", email: "ben@example.com", password: "password123"}
];
const seed_posts = [{
    title: "",
    content: "",
    username: ""
}];

export async function seed() {
    for (const user of seed_users) {
        await prisma.user.create({
            data: user,
        });
    }
}

if (import.meta.main) {
    await seed();
    console.log("Seeding finished.");
}
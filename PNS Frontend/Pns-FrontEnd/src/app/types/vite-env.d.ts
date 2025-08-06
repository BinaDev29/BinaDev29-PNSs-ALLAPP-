// src/app/types/vite-env.d.ts
/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_API_BASE_URL: string;
  // ሌሎች VITE_ የሚጀምሩ environment variables እዚህ ጋር መጨመር ትችላለህ
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
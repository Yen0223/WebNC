import React, { useEffect } from "react";
const Rss = () => {
  useEffect(() => {
    document.title = "Trang Rss Feed";
  }, []);
  return (
  <h1>
    Đây là trang Rss Feed
    </h1>
  );
}

export default Rss;

import React, { Component } from "react";
import { Carousel } from "react-bootstrap";
import { Link } from "react-router-dom";
import { getNewsForSlider } from "../../services/news";
export default function HomeSlider() {
  const [news, setNews] = React.useState([]);
  React.useEffect(() => {
    getNewsForSlider().then((data) => {
      setNews(data.data.$values);
    });
  }, []);
  const imgurl = (id, extension) => {
    const idStr = id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    return `${process.env.REACT_APP_API_URL}/pr/documents/${name}${extension}`;
  };
  return (
    <Carousel variant="light">
      {news
        ? news.map((newsValue) => (
            <Carousel.Item>
              {newsValue.homeBannerSetted ? (
                <img
                  className="d-block w-100"
                  src={imgurl(newsValue.id, newsValue.extensionHome)}
                  alt="First slide"
                />
              ) : null}
              <Carousel.Caption>
                <Link
                  eventKey="news"
                  to={{
                    pathname: "/news_detail",
                    state: { newsValue, eventKey: "news" },
                  }}
                >
                  <h5>{newsValue.title}</h5>
                </Link>
              </Carousel.Caption>
            </Carousel.Item>
          ))
        : null}
    </Carousel>
  );
}

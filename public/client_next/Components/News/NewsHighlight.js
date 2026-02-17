import {
  Card,
  CardActions,
  CardContent,
  CardMedia,
  Container,
  Grid,
  Typography,
} from "@mui/material";
import Link from "next/link";
import React from "react";
import { getNewsHighlight } from "../../api/news";
import styles from "../../styles/News.module.css";

export default function NewsHighlight({ getLocal, getLocalByVal }) {
  const [news, setNews] = React.useState([]);
  React.useEffect(() => {
    getNewsHighlight().then((data) => {
      setNews(data.data.$values);
    });
  }, []);
  const imgurl = (id, extension) => {
    const idStr = id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    return `${process.env.NEXT_PUBLIC_API_URL}/pr/documents/${name}${extension}`;
  };
  return (
    <Grid item xs={12} md={12}>
      <div className="bg">{getLocal("Recent News")}</div>
      <main>
        <Container>
          <Grid container spacing={1}>
            {news
              ? news.map((newsValue, index) => (
                  <Grid item xs={12} sm={6} md={3} key={index}>
                    <Card className={styles.card}>
                      {newsValue && newsValue.bannerSetted ? (
                        <CardMedia
                          className={styles.cardMedia}
                          image={imgurl(newsValue.id, newsValue.extension)}
                          title={getLocalByVal(
                            newsValue.title,
                            newsValue.titleLocal
                          )}
                        />
                      ) : null}
                      <CardContent className={styles.cardContent}>
                        <Typography
                          gutterBottom
                          variant="h6"
                          className={styles.title}
                          style={{fontSize: "13px"}}
                        >
                          {getLocalByVal(newsValue.title, newsValue.titleLocal)}
                        </Typography>
                        <Typography className="body" 
                          style={{fontSize: "12px"}}>
                          {getLocalByVal(newsValue.brief, newsValue.briefLocal)}
                        </Typography>
                      </CardContent>
                      <CardActions>
                          <Link
                            href={`/news/detail?news_id=${newsValue.id}`}
                            passHref={true}
                            className="btn btn-outline-success btn-sm">

                            {getLocal("Read Full News")}

                          </Link>
                      </CardActions>
                    </Card>
                  </Grid>
                ))
              : null}
            <Grid item xs={12} md={6}></Grid>
            <Grid item xs={12} md={6}></Grid>
          </Grid>
        </Container>
      </main>
    </Grid>
  );
}
